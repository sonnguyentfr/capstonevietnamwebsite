using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using NVCMS.Modules.Events;
using System.IO;
using DotNetNuke.Security;
using DotNetNuke.UI.Utilities;
using System.Collections;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.FileSystem;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using DotNetNuke.Services.Localization;
using NVCMS.Modules.ThongTinCongTy;
using HtmlAgilityPack;
using DotNetNuke.Entities.Users;

namespace DesktopModules.TinTuc.ViewPage
{
    public partial class Details : PortalModuleBase
    {

        private string setting_details_template;

        private int setting_details_other;
        private int setting_details_comment;
        private bool setting_enable_comment;
        private bool setting_enable_commentLogin;
        private bool setting_enable_other;
        //Tin lien quan
        private int setting_details_more;
        private string setting_type;
        private int setting_details_morepage;
        private int setting_imgWidth;
        private int setting_imgHeight;
        //--
        private readonly string TOKEN_NAME = "[NAME]";
        private readonly string TOKEN_USER = "[USER]";
        private readonly string TOKEN_URL = "[URL]";
        private readonly string TOKEN_NAMETITLE = "[NAMEALT]";
        private readonly string TOKEN_IMAGE = "[IMAGE]";
        private readonly string TOKEN_DATE = "[DATE]";
        private readonly string TOKEN_DIADIEM = "[DIADIEM]";
        private readonly string TOKEN_THANHPHANTHAMDU = "[THANHPHANTHAMDU]";
        private readonly string TOKEN_VIEW = "[VIEW]";
        private readonly string TOKEN_DESCRIPTION = "[DESCRIPTION]";
        private readonly string TOKEN_CONTENT = "[CONTENT]";
        private readonly string TOKEN_RELATED = "[RELATED_NEWS]";
        private readonly string TOKEN_PAGEBREAK = "[PageBreak]";
        private readonly string TOKEN_TAGS = "[TAGS]";
        private readonly string TOKEN_ATTACH = "[ATTACH_FILE]";
        private readonly string TOKEN_SOURCE = "[SOURCE]";
        private readonly string TOKEN_SOURCEPLAY = "[SOURCEPLAY]";
        private readonly string TOKEN_SFB = "[SFB]";
        private readonly string TOKEN_SFBlike = "[SFBLike]";
        //TEXT
        private readonly string TOKEN_TXT_THOIGIAN = "[TXTTHOIGIAN]";
        private readonly string TOKEN_TXT_DIADIEM = "[TXTDIADIEM]";
        private readonly string TOKEN_ANHURL = "[ANHURL]";

        #region Property

        public int ItemID
        {
            get
            {
                if (ViewState["ItemID"] != null)
                {
                    try { return Convert.ToInt32(ViewState["ItemID"]); }
                    catch { return 0; }
                }
                else
                {
                    ViewState["ItemID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["ItemID"] = value;
            }
        }
        public int TotalPage
        {
            get
            {
                if (ViewState["TotalPage"] != null)
                {
                    try { return Convert.ToInt32(ViewState["TotalPage"]); }
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
                if (ViewState["CommentCurrentPage"] != null)
                {
                    try { return Convert.ToInt32(ViewState["CommentCurrentPage"]); }
                    catch { return 0; }
                }
                else
                {
                    ViewState["CommentCurrentPage"] = 1;
                    return 1;
                }
            }
            set
            {
                ViewState["CommentCurrentPage"] = value;
            }
        }

        public int PageSize
        {
            get
            {
                if (ViewState["CommentPageSize"] != null)
                {
                    try { return Convert.ToInt32(ViewState["CommentPageSize"]); }
                    catch { return 0; }
                }
                else
                {
                    ViewState["CommentPageSize"] = setting_details_comment;
                    return setting_details_comment;
                }
            }
            set
            {
                ViewState["CommentPageSize"] = value;
            }
        }
        //public readonly DotNetNuke.Framework.CDefault BasePage
        //{
        //    get { return (DotNetNuke.Framework.CDefault)this.Page; }
        //}
        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSetting();

                string fbclid = Request.QueryString["fbclid"];
                string sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "");

                string sId = Ultis.GetRequestId2(sUrl);
                ItemID = Convert.ToInt32( sId.Remove(0, 1));
                if (setting_details_other > 0)
                {
                    // vbLastest.ItemID = ItemID;
                }

                EventsController _EventsController = new EventsController();
                //NewsAttachController attachController = new NewsAttachController();
                EventsInfo _EventsInfo = _EventsController.Events_GetByID(ItemID, PortalId);
                //NewsByMediaController _NewsByMediaController = new NewsByMediaController();
                if (_EventsInfo != null)
                {

                    DotNetNuke.Framework.CDefault cp = (DotNetNuke.Framework.CDefault)Page;
                    if (_EventsInfo.Isactive == false)
                    {
                        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId));
                    }
                    string requestedUrl = (string)HttpContext.Current.Items["UrlRewrite:OriginalUrl"];
                    int tabID = 0;
                    tabID = PortalSettings.ActiveTab.TabID;
                    //Neu danh muc khong duoc map voi tab thi mac dinh map voi page Danh muc
                    if (tabID == -1) tabID = BL.tabDanhMuc;
                    string urlFormat = Ultis.EventsFormatLink(tabID, ItemID.ToString(), _EventsInfo.Title);//Ultis.FormatLink(tabID, ItemID, _EventsInfo.Title);
                    cp.Title = _EventsInfo.Title;
                    cp.Description = _EventsInfo.Descreption;
                    //string sTag = "";
                    ////Tags
                    //if (_EventsInfo.Tags != "")
                    //{
                    //    string[] sTags = _EventsInfo.Tags.Split(',');
                    //    for (int i = 0; i < sTags.Length; i++)
                    //    {
                    //        //arrTags.Add(sTags[i]);
                    //        string tagreplace = sTags[i];
                    //        tagreplace = tagreplace.Replace(" ", "+");
                    //        if (sTags[i] != "")
                    //        {
                    //            sTag += "<li><a href='/tags?tag=" + tagreplace + "'><span class='trending-span'>#</span>" + sTags[i] + "</a><li>";
                    //        }
                    //    }
                    //}
                    //Attach file
                    //lay thong tin cong ty
                    #region Metadata
                    // Doan nay lam cho the meta
                    string strmeta = "";
                    strmeta = "<link rel='stylesheet' href='/static/nvcms/css/jquery.fancybox.min.css'>"
                        + "<script src='/static/nvcms/js/jquery.fancybox.min.js'></script>"
                        + "<meta property='og:site_name' content='Tạp chí điện tử Thương Trường'/>"
                        + "<meta property='og:rich_attachment' content='true' />"
                        + "<meta property='article:publisher' content='https://www.facebook.com/thuongtruong.com.vn/' />"
                        + "<meta property='og:type' content='article' />"
                        + "<meta property='og:url' content='__metaturl__' />"
                        + "<meta property='og:title' content='___metatitle__'/>"
                        + "<meta property='og:description' content='___metades__'/>"
                        + "<meta name='description' content='___metades__'/><meta name='tags' content='___TAGs__' /><meta property='og:locale' content='vi_VN' />"
                        + "<meta property='og:image' content='___metaavatar__'/>"
                        + "<meta property='og:image:width' content='720' />"
                        + "<meta http-equiv='Pragma' content='no-cache'> "
                        + "<meta http-equiv='Expires' content='-1'> "
                        + "<meta http-equiv='cache-control' content='no-store'> "
                        + "<meta property='og:image:height' content='378' />"
                        + "<meta property='article:published_time' content='__createddate__' />"
                        + "<meta property='article:modified_time' content='__publichddate__' />"
                        + "<meta property='article:section' content='___section__' />"
                        + "<meta property='og:tag' content='___TAGs__' />"
                        + "<meta name='twitter:card' content='___metades__' />"
                        + "<meta name='twitter:description' content='___metades__' />"
                        + "<meta name='twitter:title' content='___metatitle__' />"
                        + "<link rel='alternate' type='application/rss+xml' title='___metatitle__' href='__metaturlfeed__' />"
                        + "<link rel='alternate' type='application/rss+xml' href='__metaturlfeed2__' />"
                        + "<meta name='twitter:image' content='___metaavatar__' />"
                        + "<meta name='news_keywords' content='___TAGs__' />"
                        + "<meta id='MetaKeywords' name='KEYWORDS' content='___TAGs__' />"
                        + "<link rel='alternate' href='__metaturl__' hreflang='vi-vn' />"
                        + "<link rel='canonical' href='__metaturl__' />";
                    strmeta = strmeta.Replace("___metatitle__", ReplaceChuoi.titlenews(_EventsInfo.Title));
                    strmeta = strmeta.Replace("___metaavatar__", _EventsInfo.Avatar.Replace("/DATA", BL.filesDomain));
                    strmeta = strmeta.Replace("___metades__", Ultis.SubString(_EventsInfo.Descreption, 32, ""));
                    strmeta = strmeta.Replace("__metaturlamp__", "https://" + Request.Url.Host + "/amp/" + Ultis.BuildEntryLink(_EventsInfo.id, _EventsInfo.Title.ToLower()));
                    // requestedUrl = requestedUrl.Replace("http", "https")
                    strmeta = strmeta.Replace("__metaturl__", requestedUrl);
                    //strmeta = strmeta.Replace("___TAGs__", _EventsInfo.Tags);
                    strmeta = strmeta.Replace("___section__", _EventsInfo.CategoryName);
                    strmeta = strmeta.Replace("__createddate__", _EventsInfo.Createddate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    strmeta = strmeta.Replace("__publichddate__", _EventsInfo.Createddate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    //string sResult1 = "";
                    //if (!string.IsNullOrEmpty(_EventsInfo.Tags))
                    //{
                    //    string[] strArr = Regex.Split(_EventsInfo.Tags, ",");
                    //    for (int i = 0; i <= strArr.Count() - 1; i++)
                    //        sResult1 += "<meta property=\"article:tag\" content=\"" + ReplaceChuoi.bodau2(strArr[i]) + "\"/>";
                    //}
                    //strmeta = strmeta.Replace("__GOOGLEBOOT", "<meta name='robots' content='index,follow' /><meta name='Googlebot' content='index,follow' /><link rel='amphtml' href='https://" + Request.Url.Host + "/amp/" + Ultis.BuildEntryLink(_EventsInfo.NewId, _EventsInfo.Title.ToLower()) + ".html'/>");
                    //strmeta = strmeta.Replace("___ogTAGS___", sResult1);
                    var htmlHeaderTags = "";
                    LiteralControl htmlHeaderCtrl = new LiteralControl();
                    htmlHeaderTags = strmeta;
                    htmlHeaderCtrl.Text = htmlHeaderTags.ToString();
                    Page.Header.Controls.Add(htmlHeaderCtrl);
                    // --Boot google
                    string strgoogle;
                    strgoogle = "<script type=\"application/ld+json\">"
                    + "{"
                    + "\"@context\": \"http://schema.org\","
                    + "\"@type\": \"NewsArticle\","
                   + " \"mainEntityOfPage\":{"
                   + "     \"@type\":\"WebPage\","
                        + "\"@id\":\"__metaturl__\""
                    + "},"
                    + "\"headline\": \"___metatitle__\","
                    + "\"description\": \"___metades__\","
                    + "\"image\": {"
                        + "\"@type\": \"ImageObject\","
                        + "\"url\": \"___metaavatar__\","
                     + "   \"width\" : 800,"
                     + "   \"height\" : 800    },"
                   + " \"datePublished\": \"___createddate__\","
                   + " \"dateModified\": \"___createddate2__\","
                   + " \"author\": {"
                     + "   \"@type\": \"Person\","
                     + "   \"name\": \"___uuser__\""
                   + " },"
                   + " \"publisher\": {"
                      + "  \"@type\": \"Organization\","
                       + " \"name\": \"__Domain__\","
                       + " \"logo\": {"
                          + "  \"@type\": \"ImageObject\","
                          + "  \"url\": \"__Logo___\","
                          + "  \"width\": 300,"
                          + "  \"height\": 48"
                        + "}"
                    + "}"
                + "}"
                + "</script>";

                    strgoogle = strgoogle.Replace("__metaturl__", requestedUrl);
                    strgoogle = strgoogle.Replace("___metatitle__", ReplaceChuoi.titlenews(_EventsInfo.Title));
                    strgoogle = strgoogle.Replace("___metades__", ReplaceChuoi.titlenews(_EventsInfo.Descreption));
                    strgoogle = strgoogle.Replace("___metaavatar__", _EventsInfo.Avatar.Replace("/DATA", BL.filesDomain));
                    strgoogle = strgoogle.Replace("___createddate__", _EventsInfo.Createddate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    strgoogle = strgoogle.Replace("___createddate2__", _EventsInfo.Createddate.ToString("yyyy-MM-ddTHH:mm:sszzz"));

                    var htmlHeaderTags1 = "";
                    LiteralControl htmlHeaderCtrl1 = new LiteralControl();
                    htmlHeaderTags1 = strgoogle;
                    htmlHeaderCtrl1.Text = htmlHeaderTags1.ToString();
                    Page.Header.Controls.Add(htmlHeaderCtrl1);
                    string strgoogleindex;
                    strgoogleindex = "<script type=\"application/ld+json\">"
            + "{"
                + "\"@context\": \"http://schema.org\","
                + "\"@type\": \"BreadcrumbList\","
                + "\"itemListElement\": ["
                    + "{"
                        + "\"@type\": \"ListItem\","
                        + "\"position\": 1,"
                        + "\"item\": {"
                            + "\"@id\": \"__Domain__\","
                            + "\"name\": \"Trang chủ\""
                        + "}"
                    + "},___StringCAT__"
                + "]"
            + "}"
            + "</script>";

                    string strgoogleindex1;
                    strgoogleindex1 = "{"
                        + "\"@type\": \"ListItem\","
                        + "\"position\": 2,"
                        + "\"item\": {"
                            + "\"@id\": \"__metaturl1__\","
                            + "\"name\": \"___metatitle__c1\""
                        + "}"
                    + "}";

                    string strgoogleindex2;
                    strgoogleindex2 = "{"
                        + "\"@type\": \"ListItem\","
                        + "\"position\": 3,"
                        + "\"item\": {"
                            + "\"@id\": \"__metaturl2__\","
                            + "\"name\": \"___metatitle__c2\""
                        + "}"
                    + "}";


                    string strgoogleindex4;
                    strgoogleindex4 = "{"
                        + "\"@type\": \"ListItem\","
                        + "\"position\": 4,"
                        + "\"item\": {"
                            + "\"@id\": \"__metaturl4__\","
                            + "\"name\": \"___metatitle__c4\""
                        + "}"
                    + "}";
                    
                    #endregion Metadata
                    //Fill du lieu vao template
                    string sTemplate = "";
                    string sTemplateFile = Server.MapPath("/Portals/0/EventsTemplates/") + setting_details_template;
                    if (File.Exists(sTemplateFile))
                        sTemplate = File.ReadAllText(sTemplateFile);
                    string sContent = "";
                    sContent += Server.HtmlDecode( _EventsInfo.Descreption);
                    //Tag
                    string sTag = "";
                    //LAY THONGT IN USER
                    string usercreate = "";
                    usercreate = BL.GetNameByUserId(PortalId, _EventsInfo.UserId);
                    //Share Social Botton
                    string fbsharelike = "<div class='fb-like' data-size='large' data-href='" + requestedUrl + "'data-layout='button_count' data-action='like' data-size='small' data-show-faces='false' data-share='false'></div>";
                    string fbshare = "<div class='fb-share-button' data-layout='button_count' data-size='large' data-href='" + requestedUrl + "'></div>";
                    sTemplate = sTemplate.Replace(TOKEN_NAME, _EventsInfo.Title).Replace(TOKEN_USER, usercreate).Replace(TOKEN_URL, requestedUrl).Replace(TOKEN_NAMETITLE, ReplaceChuoi.titlenews(_EventsInfo.Title)).Replace(TOKEN_IMAGE, _EventsInfo.Avatar.Replace("/DATA", BL.filesDomain)).Replace(TOKEN_DATE, BL.FormatDate(_EventsInfo.enddatetime)).Replace(TOKEN_DIADIEM, _EventsInfo.diadiem).Replace(TOKEN_THANHPHANTHAMDU, _EventsInfo.thanhphan).Replace(TOKEN_DESCRIPTION, Server.HtmlDecode(_EventsInfo.Descreption)).Replace(TOKEN_CONTENT, Server.HtmlDecode(sContent))
                        .Replace(TOKEN_SFBlike, fbsharelike).Replace(TOKEN_SFB, fbshare).Replace(TOKEN_TAGS, sTag)
                        .Replace(TOKEN_TXT_THOIGIAN, Localization.GetSafeJSString("thoigian.text", Ultis.resourceevents))
                        .Replace(TOKEN_TXT_DIADIEM, Localization.GetSafeJSString("diadiem.text", Ultis.resourceevents));
                    ltContent.Text = sTemplate != "" ? sTemplate : setting_details_template + "Module này chưa được áp dụng Template. Vui lòng chọn Template !";

                    //Tin khac cung chuyen muc
                    if (setting_details_more > 0)
                    {
                        //vbRelated.TotalItem = setting_details_more;
                        //vbRelated.ItemID = ItemID;
                        //vbRelated.Title = "Tin khác";
                        //if (setting_type == "Scroll")
                        //{
                        //    //vbRelated.scroll = true;
                        //    //vbRelated.scrollpage = setting_details_morepage;
                        //    //vbRelated.setting_imgWidth = setting_imgWidth;
                        //    //vbRelated.setting_imgHeight = setting_imgHeight;
                        //}
                        //else
                        //{

                        //}
                        ArrayList arr2 = _EventsController.Events_FindShow_Index(ItemID.ToString(),PortalId, 1,1, setting_details_more);
                        if (arr2 != null && arr2.Count > 0)
                        {
                            if (DotNetNuke.Common.Utilities.DataCache.GetCache(BL.NewsHomeCat + "sukienlienquan") == null)
                            {
                                DotNetNuke.Common.Utilities.DataCache.SetCache(BL.NewsHomeCat +"sukienlienquan", arr2, null, DateTime.Now.AddSeconds(10), TimeSpan.Zero);
                                drgOtherNews.DataSource = arr2;
                                drgOtherNews.DataBind();
                            }
                            else
                            {
                                drgOtherNews.DataSource = DotNetNuke.Common.Utilities.DataCache.GetCache(BL.NewsHomeCat + "sukienlienquan");
                                drgOtherNews.DataBind();
                            }
                        }

                    }
                    
                }
                // ltContent.Text = "details";
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
                //Details Template
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_Template.ToString()]))
                {
                    setting_details_template = ModuleConfiguration.ModuleSettings[BL.settingDetails_Template.ToString()].ToString();
                }
                else isNull = true; if (isNull) return isNull;

                //Tin lien quan - soluong
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_More.ToString()]))
                {
                    setting_details_more = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingDetails_More.ToString()]);
                }
                else isNull = true; if (isNull) return isNull;
                //Tin lien quan - sotrang
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_MorePage.ToString()]))
                {
                    setting_details_morepage = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingDetails_MorePage.ToString()]);
                }
                else isNull = true; if (isNull) return isNull;
                //Tin lien quan kieu
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingView_Type.ToString()]))
                {
                    setting_type = Convert.ToString(ModuleConfiguration.ModuleSettings[BL.settingView_Type.ToString()]);
                }
                else isNull = true; if (isNull) return isNull;
                //Tin lien quan Image size
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()]))
                {
                    string[] sImgSize = ModuleConfiguration.ModuleSettings[BL.settingList_ImgSize.ToString()].ToString().Split(';');
                    setting_imgWidth = Convert.ToInt32(sImgSize[0]);
                    setting_imgHeight = Convert.ToInt32(sImgSize[1]);
                }
                else isNull = true; if (isNull) return isNull;
                //Details comment
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_Comment.ToString()]))
                {
                    setting_details_comment = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingDetails_Comment.ToString()]);
                    PageSize = setting_details_comment;
                }
                else isNull = true; if (isNull) return isNull;

                //Details other
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_Other.ToString()]))
                {
                    setting_details_other = Convert.ToInt32(ModuleConfiguration.ModuleSettings[BL.settingDetails_Other.ToString()]);
                }
                else isNull = true; if (isNull) return isNull;

                //Social, Comment
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingDetails_Allow.ToString()]))
                {
                    setting_enable_comment = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[BL.settingDetails_Allow.ToString()]);
                }
                else isNull = true; if (isNull) return isNull;
                //Social, Comment
                if (!Null.IsNull(ModuleConfiguration.ModuleSettings["settingDetails_AllowLogin"]))
                {
                    setting_enable_commentLogin = Convert.ToBoolean(ModuleConfiguration.ModuleSettings["settingDetails_AllowLogin"]);
                }
                else isNull = true; if (isNull) return isNull;
                //enable other
                //if (!Null.IsNull(ModuleConfiguration.ModuleSettings[BL.settingList_ShowOtherNews.ToString()]))
                //{
                //    setting_enable_other = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[BL.settingList_ShowOtherNews.ToString()]);
                //}
                else isNull = true; if (isNull) return isNull;
            }
            catch (Exception ex) { ltContent.Text = ex.Message; }
            return isNull;
        }
        private string FillterInput(string sInput)
        {
            var objSecurity = new PortalSecurity();
            string sReturn = System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(sInput, PortalSecurity.FilterFlag.NoScripting), "<[^>]*>", "");
            sReturn = System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(sReturn, PortalSecurity.FilterFlag.NoMarkup), "<[^>]*>", "");
            sReturn = System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(sReturn, PortalSecurity.FilterFlag.NoSQL), "<[^>]*>", "");
            sReturn = System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(sReturn, PortalSecurity.FilterFlag.NoAngleBrackets), "<[^>]*>", "");
            return sReturn;
        }

    }
}