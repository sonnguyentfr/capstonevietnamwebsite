using DotNetNuke.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using NVCMS.Modules.Video;
using System.IO;
using System.Collections;
using DotNetNuke.Entities.Tabs;
using System.Globalization;
using NVCMSMVC.Web.Components;
using System.Web.Mvc;
using System.Diagnostics;
using NVCMS.Web.Components;
using DotNetNuke.Services.Localization;

namespace DesktopModules.Video.ViewPage
{
    public partial class Index : PortalModuleBase
    {

        private int setting_pageSize;
        private int setting_imgWidth;
        private int setting_imgHeight;
        private bool setting_showPage;

        private string setting_template;
        private string setting_order;

        private readonly string TOKEN_LIST_ITEM = "LIST_ITEM";
        private readonly string TOKEN_NAME = "[NAME]";
        private readonly string TOKEN_USER = "[USER]";
        private readonly string TOKEN_NAMETITLE = "[NAMEALT]";
        private readonly string TOKEN_NEWID = "[NEWID]";
        private readonly string TOKEN_URL = "[URL]";
        private readonly string TOKEN_IMAGE = "[IMAGE]";
        private readonly string TOKEN_IMAGEHEIGHT = "[IMAGEHEIGHT]";
        private readonly string TOKEN_IMAGEWIDTH = "[IMAGEWIDTH]";
        private readonly string TOKEN_DATE = "[DATE]";
        private readonly string TOKEN_DESCRIPTION = "[DESCRIPTION]";
        private readonly string TOEKN_CONTENT = "[CONTENT]";

        #region Property
        public int TotalPage
        {
            get
            {
                if (ViewState["TotalPage"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(ViewState["TotalPage"]);
                    }
                    catch { return Null.NullInteger; }
                }
                else
                {
                    ViewState["TotalPage"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["TotalPage"] = value;
            }
        }
        public int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(ViewState["CurrentPage"]);
                    }
                    catch { return Null.NullInteger; }
                }
                else
                {
                    ViewState["CurrentPage"] = 1;
                    return 1;
                }
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(ViewState["PageSize"]);
                    }
                    catch { return Null.NullInteger; }
                }
                else
                {
                    ViewState["PageSize"] = setting_pageSize;
                    return setting_pageSize;
                }
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }
        public int TotalRecord
        {
            get
            {
                if (ViewState["TotalRecord"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(ViewState["TotalRecord"]);
                    }
                    catch { return Null.NullInteger; }
                }
                else
                {
                    ViewState["TotalRecord"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["TotalRecord"] = value;
            }
        }
        public int CategoryID
        {
            get
            {
                if (ViewState["CategoryId"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(ViewState["CategoryId"]);
                    }
                    catch { return Null.NullInteger; }
                }
                else
                {
                    ViewState["CategoryId"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }
        public string SubtractIds
        {
            get
            {
                if (Session["SubtractIds_" + PortalSettings.ActiveTab.ToString()] != null)
                {
                    return Session["SubtractIds_" + PortalSettings.ActiveTab.ToString()].ToString();
                }
                else
                {
                    Session["SubtractIds_" + PortalSettings.ActiveTab.ToString()] = "";
                    return "";
                }
            }
            set
            {
                Session["SubtractIds_" + PortalSettings.ActiveTab.ToString()] = value;
            }
        }
        #endregion Property
        protected override void OnLoad(EventArgs e)
        {
            DateTime dt = DateTime.Now.AddDays(10);
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetExpires(dt);
            Response.Cache.SetMaxAge(new TimeSpan(dt.Ticks - DateTime.Now.Ticks));
            Response.ClearHeaders();
            base.OnLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();
                    if (LoadSetting()) { ltContent.Text = "Load cấu hình module lỗi !"; return; }
                    if (Request.QueryString["trang"] != null)
                        CurrentPage = Convert.ToInt32(Request.QueryString["trang"]);
                    ltContent.Text = BindData();
                    //sw.Stop();
                    //ltrllia.Text = sw.ElapsedMilliseconds.ToString() + "-ms";
                }
                else
                {
                    string sTemp = Request["__EVENTARGUMENT"];
                    if (String.IsNullOrEmpty(sTemp) && sTemp.StartsWith("Page_"))
                    {
                        CurrentPage = Convert.ToInt32(sTemp.Replace("Page_", ""));
                        ltContent.Text = BindData();
                    }
                }
            }
            catch (Exception ex) { ltContent.Text = ex.Message; }
        }
        [CompressContent]
        [OutputCache(Duration = 60, VaryByParam = "*")]
        private string BindData()
        {

            Videos_Controller controller = new Videos_Controller();
            string sessionSubHotCat = "";
            if (Session["SubtractIdsHotCat"] != null)
            {
                sessionSubHotCat = Session["SubtractIdsHotCat"].ToString();
            }
            TotalRecord = controller.Find_Show_Count(0);
            int totalPage = TotalRecord % PageSize != 0 ? (TotalRecord / PageSize + 1) : (TotalRecord / PageSize);
            if (totalPage > 1 && setting_showPage)
            {
                vbPaging.TotalPage = totalPage;
                vbPaging.bindPages();
                vbPaging.Visible = true;
            }
            else vbPaging.Visible = false;

            string cachestring = "VideoTemplatez" + setting_template + CurrentPage + ModuleId + TabId;
            Hashtable cache = DataCache.GetCache<Hashtable>(cachestring);
            if ((cache == null))
                cache = new Hashtable();
            if (!cache.ContainsKey(cachestring))
            {
                //Lay thong tin cat
                string sTemplate = "";
                string sTemplateFile = Server.MapPath("/Portals/0/VideoTemplate/") + setting_template;
                if (File.Exists(sTemplateFile))
                    sTemplate = File.ReadAllText(sTemplateFile);
                
                ArrayList list = new ArrayList();
                string cachename = BL.NewsCatList + CategoryID + CurrentPage + ModuleId + TabId;
                list = controller.Find_Show_Index(0, CurrentPage, PageSize);

                if (DataCache.GetCache(cachename) == null)
                {
                    DataCache.SetCache(cachename, list, null, DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                }

                if (TotalRecord == 0)
                {
                    sTemplate = "";
                }
                else
                {
                    string sTemplate_list = sTemplate.Contains(TOKEN_LIST_ITEM) ? TrimToken(sTemplate, TOKEN_LIST_ITEM) : "";
                    string sListItem = "";
                    for (int i = 0; i < list.Count; i++)
                    {
                        Videos_Info news = (Videos_Info)list[i];
                        sListItem += ToHTML(sTemplate_list, news, (i + 1), setting_imgWidth, setting_imgHeight);
                    }
                    sTemplate = sTemplate.Replace(sTemplate_list, sListItem).Replace("[" + TOKEN_LIST_ITEM + "]", "").Replace("[/" + TOKEN_LIST_ITEM + "]", "");

                }
                cache[cachestring] = sTemplate.ToString();
                //ltContent.Text = sTemplate;
                if (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching != DotNetNuke.Common.Globals.PerformanceSettings.NoCaching)
                    DataCache.SetCache(cachestring, cache);
            }
            return cache[cachestring].ToString();

            //ltContent.Text = sTemplate;
        }
        private string ToHTML(string sTemplate, Videos_Info news, int position, int imgWidth, int imgHeight)
        {
            try
            {
                
                string url = "#";
                url = Ultis.FormatLinkVideo(PortalSettings.Current.ActiveTab.TabID, news.VideoId, news.Title);
                string title = news.Title;
                string titleatl = ReplaceChuoi.titlenews(news.Title);
                string newid = Convert.ToString(news.VideoId);
                string date = "";
                if (BL.GetLanguage() == "en-US")
                {
                    date = news.PublishedDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    date = news.PublishedDate.ToString("dd/MM/yyyy");
                }
               
                string image = "";
                string datecount = Ultis.ToRelativeDate(news.PublishedDate);
                if (news.ImagePath != null)
                {
                    if (imgWidth != 0 && imgHeight != 0)
                        image = Ultis.FormatThumbImage(news.ImagePath, imgWidth, imgHeight, "crop", "topcenter", "");
                }

                string description = BL.RemoveHTMLTags(news.Summary).Replace("<", "").Replace(">", "");
                
                string usercreate = "";
                usercreate = BL.GetNameByUserId(0, news.UserId);
                return sTemplate.Replace(TOKEN_DESCRIPTION, description).Replace(TOKEN_IMAGE, image).Replace(TOKEN_IMAGEHEIGHT, Convert.ToString(imgHeight)).Replace(TOKEN_IMAGEWIDTH, Convert.ToString(imgWidth)).Replace(TOKEN_DATE, date)
                    .Replace(TOKEN_NEWID, newid).Replace(TOKEN_USER, usercreate)
                    .Replace(TOKEN_NAME, title).Replace(TOKEN_NAMETITLE, titleatl).Replace(TOKEN_URL, url).Replace(TOEKN_CONTENT, news.Content);
            }
            catch
            {
                return "";
            }
        }
        private string TrimToken(string sInput, string sToken)
        {
            try
            {
                string sStart = "[" + sToken + "]";
                string sEnd = "[/" + sToken + "]";
                if (!sInput.Contains(sStart) || !sInput.Contains(sEnd)) return "";

                int startIndex = sInput.IndexOf(sStart, StringComparison.CurrentCultureIgnoreCase) + sStart.Length;
                int endIndex = sInput.IndexOf(sEnd, startIndex, StringComparison.CurrentCultureIgnoreCase);
                int length = endIndex - startIndex;

                return sInput.Substring(startIndex, length);
            }
            catch { return ""; }
        }
        private bool LoadSetting()
        {
            bool isNull = false;
            try
            {
               
                //Page size
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_PageSize.ToString()]))
                {
                    setting_pageSize = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingList_PageSize.ToString()]);
                    PageSize = setting_pageSize;
                }
                else isNull = true; if (isNull) return isNull;

                //Image size
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()]))
                {
                    string[] sImgSize = ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()].ToString().Split(';');
                    setting_imgWidth = Convert.ToInt32(sImgSize[0]);
                    setting_imgHeight = Convert.ToInt32(sImgSize[1]);
                }
                else isNull = true; if (isNull) return isNull;

                //Page Template
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_Template.ToString()]))
                {
                    setting_template = ModuleConfiguration.ModuleSettings[BL.settingList_Template.ToString()].ToString();
                }
                else isNull = true; if (isNull) return isNull;


                //Show page
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ShowPage.ToString()]))
                {
                    setting_showPage = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[BL.settingList_ShowPage]);
                }
                else setting_showPage = true;
            }
            catch { return isNull; }
            return isNull;
        }

        private void PrintSettings()
        {
            string sSettings = "";
            if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_PageSize.ToString()]))
            {
                sSettings += "PageSize =" + ModuleConfiguration.ModuleSettings[BL.settingList_PageSize.ToString()] + "<br/>";
            }

            if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()]))
            {
                sSettings += "ImgSize =" + ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()].ToString() + "<br/>";
            }

            if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_Template.ToString()]))
            {
                sSettings += "Template =" + ModuleConfiguration.ModuleSettings[BL.settingList_Template.ToString()].ToString() + "<br/>";
            }
            ltSettings.Text = sSettings;
        }
    }
}