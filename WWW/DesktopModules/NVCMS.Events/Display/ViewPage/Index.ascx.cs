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
using NVCMS.Modules.Events;
using System.IO;
using System.Collections;
using DotNetNuke.Entities.Tabs;
using System.Globalization;
using NVCMSMVC.Web.Components;
using System.Web.Mvc;
using System.Diagnostics;
using NVCMS.Web.Components;

namespace DesktopModules.TinTuc.ViewPage
{
    public partial class Index : PortalModuleBase
    {
        private int setting_hot;
        private int setting_imgWidthHOT;
        private int setting_imgHeightHOT;

        private int setting_pageSize;
        private int setting_imgWidth;
        private int setting_imgHeight;
        private int setting_sizeDes;
        private bool setting_showPage;

        private string setting_template;
        private string setting_order;

        private readonly string TOKEN_LIST_ITEM = "LIST_ITEM";
        private readonly string TOKEN_HOT_ITEM = "LIST_HOT";
        private readonly string TOKEN_NAME = "[NAME]";
        private readonly string TOKEN_USER = "[USER]";
        private readonly string TOKEN_NAMETITLE = "[NAMEALT]";
        private readonly string TOKEN_NEWID = "[NEWID]";
        private readonly string TOKEN_URL = "[URL]";
        private readonly string TOKEN_CATNAME = "[CATNAME]";
        private readonly string TOKEN_REGLINK = "[REGLINK]";
        private readonly string TOKEN_CATURL = "[CATURL]";
        private readonly string TOKEN_IMAGE = "[IMAGE]";
        private readonly string TOKEN_IMAGEHEIGHT = "[IMAGEHEIGHT]";
        private readonly string TOKEN_IMAGEWIDTH = "[IMAGEWIDTH]";
        private readonly string TOKEN_DATE = "[DATE]";
        private readonly string TOKEN_THU = "[THU]";
        private readonly string TOKEN_NGAY = "[NGAY]";
        private readonly string TOKEN_NAM = "[YEAR]";
        private readonly string TOKEN_ATTACH = "[ATTACH_FILE]";
        private readonly string TOKEN_EXPIRED_DATE = "[EXPIRED_DATE]";
        private readonly string TOKEN_DESCRIPTION = "[DESCRIPTION]";
        private readonly string TOKEN_POSITION = "[POSITION]";
        private readonly string TOEKN_CONTENT = "[CONTENT]";
        private readonly string TOKEN_DATECOUNT = "[DATECOUNT]";
        private readonly string TOKEN_LIST_PAGE = "LIST_PAGE";
        private readonly string TOKEN_PAGE_PREVIOUS = "[PREVIOUS]";
        private readonly string TOKEN_PAGE_NEXT = "[NEXT]";
        private readonly string TOKEN_PAGE_LAST = "[LAST]";
        private readonly string TOKEN_PAGE_FIRST = "[FIRST]";
        private readonly string TOKEN_PAGE_INDEX = "[INDEX]";
        private readonly string TOKEN_SOURCEDOMAIN = "[SOURCEDOMAIN]";
        private readonly string TOKEN_SOURCE = "[SOURCE]";
        private readonly string TOKEN_TAG = "[TAG]";

        private readonly string ORDER_DATE_DESC = "DATE_DESC";
        private readonly string ORDER_TITLE_ASC = "TITLE_ASC";
        private readonly string ORDER_TITLE_DESC = "TITLE_DESC";
        private readonly string ORDER_VIEW_ASC = "VIEW_ASC";
        private readonly string ORDER_VIEW_DESC = "VIEW_DESC";

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
                    if (Request.QueryString["cateId"] == null)
                    {
                        string fbclid = Request.QueryString["fbclid"];
                        string sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "");

                        string sId = Ultis.GetRequestId2(sUrl);
                        CategoryID = Convert.ToInt32(sId.Remove(0, 1));
                        //CategoryID = BL.GetMappingCategoryIDByTabID(PortalSettings.ActiveTab.TabID);
                        string requestedUrl = (string)HttpContext.Current.Items["UrlRewrite:OriginalUrl"];
                        string targetUrl = Globals.NavigateURL(PortalSettings.ActiveTab.TabID);
                        if (CurrentPage > 1)
                        {
                            targetUrl += "?trang=" + CurrentPage.ToString();
                        }
                        if (requestedUrl != targetUrl)
                        {
                            Response.Redirect(targetUrl, true);
                        }
                    }
                    else
                    {

                    }
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
            EventsController controller = new EventsController();
            string sessionSubHotCat = "";
            if (Session["SubtractIdsHotCat"] != null)
            {
                sessionSubHotCat = Session["SubtractIdsHotCat"].ToString();
            }
            TotalRecord = controller.Events_FindShow_Count(sessionSubHotCat, PortalId, 1);
            int totalPage = TotalRecord % PageSize != 0 ? (TotalRecord / PageSize + 1) : (TotalRecord / PageSize);

            if (totalPage > 1 && setting_showPage)
            {
                vbPaging.TotalPage = totalPage;
                vbPaging.bindPages();
                vbPaging.Visible = true;
            }
            else vbPaging.Visible = false;

            string cachestring = "Template" + setting_template + CurrentPage + CategoryID + ModuleId;
            Hashtable cache = DataCache.GetCache<Hashtable>(cachestring);
            if ((cache == null))
                cache = new Hashtable();
            if (!cache.ContainsKey(cachestring))
            {
                //Lay thong tin cat
                string sTemplate = "";
                string sTemplateFile = Server.MapPath("/Portals/0/EventsTemplates/") + setting_template;
                if (File.Exists(sTemplateFile))
                    sTemplate = File.ReadAllText(sTemplateFile);

                //Lay tin nong

                ArrayList list = new ArrayList();
                string cachename = BL.NewsCatList + "sukien" + CategoryID + CurrentPage + ModuleId;
                list = controller.Events_FindShow_Index(sessionSubHotCat, PortalId, 1, CurrentPage, PageSize);
                if (DataCache.GetCache(cachename) == null)
                {
                    DataCache.SetCache(cachename, list, null, DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                }

                if (list.Count == 0)
                {
                    ltContent.Text = "Nội dung trống !";
                }
                else
                {

                    string sTemplate_list = sTemplate.Contains(TOKEN_LIST_ITEM) ? TrimToken(sTemplate, TOKEN_LIST_ITEM) : "";
                    string sListItem = "";
                    for (int i = 0; i < list.Count; i++)
                    {
                        EventsInfo news = (EventsInfo)list[i];
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
        private string ToHTML(string sTemplate, EventsInfo news, int position, int imgWidth, int imgHeight)
        {
            try
            {
                int tabID = TabId;
                //Neu chua map category den tab thi lay tab mac dinh la tab 'danh muc'
                if (tabID == -1 || tabID == null) tabID = BL.tabDanhMuc;
                string url = "#";
                url = Ultis.FormatLink(tabID, news.id, news.Title);
                string title = news.Title;
                string titleatl = ReplaceChuoi.titlenews(news.Title);
                string newid = Convert.ToString(news.id);
                string date = news.fromdatetime.ToShortDateString();
                string thu = news.fromdatetime.ToString("dd");
                string nam = news.fromdatetime.ToString("yyyy");
                string reglink = "";
                string ngay = news.fromdatetime.ToString("MM", new CultureInfo("vi-VN"));
                string expiredDate = ""; // news.ExpiredDate.ToShortDateString();
                string image = "";
                string datecount = "";
                string attachFile = "";
                if (imgWidth != 0 && imgHeight != 0)
                    image = Ultis.FormatThumbImage(news.Avatar, imgWidth, imgHeight, "crop", "topcenter", "");
                string description = BL.RemoveHTMLTags(news.Descreption).Replace("<", "").Replace(">", "");
                if (setting_sizeDes != 0 && description != "")
                {
                    description = Ultis.SubString(description, setting_sizeDes, "...");
                }
                string sourcedomain = "";
                
                string tag = "";
                //Tags
                
                string source ="";
                string usercreate = "";
                usercreate = BL.GetNameByUserId(PortalId, news.UserId);
                return sTemplate.Replace(TOKEN_DESCRIPTION, description).Replace(TOKEN_DATECOUNT, datecount).Replace(TOKEN_IMAGE, image).Replace(TOKEN_IMAGEHEIGHT, Convert.ToString(imgHeight)).Replace(TOKEN_IMAGEWIDTH, Convert.ToString(imgWidth)).Replace(TOKEN_DATE, date)
                    .Replace(TOKEN_NEWID, newid).Replace(TOKEN_USER, usercreate).Replace(TOKEN_TAG, tag)
                    .Replace(TOKEN_THU, thu).Replace(TOKEN_NGAY, ngay).Replace(TOKEN_NAM, nam).Replace(TOKEN_NAME, title).Replace(TOKEN_SOURCE, source).Replace(TOKEN_SOURCEDOMAIN, sourcedomain).Replace(TOKEN_NAMETITLE, titleatl).Replace(TOKEN_REGLINK, reglink).Replace(TOKEN_URL, url).Replace(TOKEN_POSITION, position.ToString()).Replace(TOKEN_EXPIRED_DATE, expiredDate).Replace(TOKEN_ATTACH, attachFile).Replace(TOEKN_CONTENT, news.Descreption);
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
                //Image size
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ImgSizeHOT.ToString()]))
                {
                    string[] sImgSize = ModuleConfiguration.ModuleSettings[BL.settingList_ImgSizeHOT.ToString()].ToString().Split(';');
                    setting_imgWidthHOT = Convert.ToInt32(sImgSize[0]);
                    setting_imgHeightHOT = Convert.ToInt32(sImgSize[1]);
                }
                else isNull = true; if (isNull) return isNull;
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

                //Order
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_Order.ToString()]))
                {
                    setting_order = ModuleConfiguration.ModuleSettings[BL.settingList_Order.ToString()].ToString();
                }
                else setting_order = "DATE_DESC";

                //Size Des
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_SizeDes.ToString()]))
                {
                    setting_sizeDes = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingList_SizeDes.ToString()].ToString());
                }
                else setting_sizeDes = 0;

                //Show page
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ShowPage.ToString()]))
                {
                    setting_showPage = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[BL.settingList_ShowPage]);
                }
                else setting_showPage = true;
            }
            catch (Exception ex) { ltContent.Text = ex.Message; }
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