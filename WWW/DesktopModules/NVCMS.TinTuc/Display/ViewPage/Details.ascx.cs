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
using NVCMS.Modules.TinTuc;
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
//using NVCMS.Modules.ThongTinCongTy;
//using HtmlAgilityPack;
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
        public string resourceform = "~/DesktopModules/TinTuc/App_LocalResources/Tintuc.ascx.resx";
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
        public int Status
        {
            get
            {
                if (ViewState["Status"] != null)
                {
                    try { return Convert.ToInt32(ViewState["Status"]); }
                    catch { return 0; }
                }
                else
                {
                    ViewState["Status"] = CommentStatus.Pulished;
                    return (int)CommentStatus.Pulished;
                }
            }
            set
            {
                ViewState["Status"] = value;
            }
        }
        //public readonly DotNetNuke.Framework.CDefault BasePage
        //{
        //    get { return (DotNetNuke.Framework.CDefault)this.Page; }
        //}
        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
            {
                LoadSetting();

                string fbclid = Request.QueryString["fbclid"];
                string sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "");
                ItemID = nvcmsBL.GetRequestId(sUrl);
                if (setting_details_other > 0)
                {
                    // vbLastest.ItemID = ItemID;
                }

                NV_NewsController newsController = new NV_NewsController();
				NV_NewsCategoriesController newscat = new NV_NewsCategoriesController();
                // NewsAttachController attachController = new NewsAttachController();
                NV_NewsInfo objNews = newsController.GetByID(ItemID);
                if (objNews != null)
                {

                    DotNetNuke.Framework.CDefault cp = (DotNetNuke.Framework.CDefault)Page;
                    if (objNews.Status != 2)
                    {
                        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId));
                    }
                    if (objNews.isActive == false)
                    {
                        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId));
                    }
                    //'Chua dc XB -> redirect nguoc lai
                    if (objNews.PublishedDate > DateTime.Now)
                    {
                        Response.Redirect(Convert.ToString(Request.UrlReferrer));
                    }
					NV_NewsCategoriesInfo objcat = newscat.GetByID(objNews.CategoryId);
                    if (objcat != null)
                    {
                        if (objcat.IsActive = false)
                        {
                            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId));
                        }
                    }
                    string requestedUrl = (string)HttpContext.Current.Items["UrlRewrite:OriginalUrl"];
                    int tabID = 0;
                    tabID = BL.GetMappingTabIDByCategoryID(objNews.CategoryId);
                    //Neu danh muc khong duoc map voi tab thi mac dinh map voi page Danh muc
                    if (tabID == -1) tabID = BL.tabDanhMuc;

                    string urlFormat = Ultis.FormatLink(tabID, ItemID, objNews.Title);
                    Response.Write(requestedUrl + " -requestedUrl<br />");
                    Response.Write(urlFormat + " -urlFormat<br />");
                    //if (requestedUrl != urlFormat)
                    //{
                    //    if (Request.QueryString["page"] == null)
                    //        Response.Redirect(Ultis.FormatLink(tabID, ItemID, objNews.Title), true);
                    //}
                    //if (tabID == BL.tabDanhMuc)
                    //{
                    //    NV_NewsCategoriesController cateController = new NV_NewsCategoriesController();
                    //    NV_NewsCategoriesInfo category = cateController.GetByID(objNews.CategoryId);
                    //    ModuleConfiguration.ModuleTitle = category.CategoryName;
                    //}
                    cp.Title = objNews.Title;
                    cp.Description = objNews.Summary;
                    
                    string sAttach = "";
                    if (!String.IsNullOrEmpty(objNews.AttachedFiles))
                        sAttach = "<a class='attach' target='_blank' href='" + objNews.AttachedFiles + "'>Tải file đính kèm</a>";
                    string sRelated = "<ul class='list_related'>";
                    //Tin chon
                    if (objNews.Links != "")
                    {
                        string[] sLinks = objNews.Links.Split(';');
                        for (int i = 0; i < sLinks.Length; i++)
                        {
                            int newId;
                            if (int.TryParse(sLinks[i], out newId))
                            {
                                var news = newsController.GetByID(newId);
                                sRelated += "<li><a title='" + ReplaceChuoi.titlenews(news.Title) + "' href='" + Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(news.CategoryId), news.NewId, news.Title) + "'>" + news.Title + "</a></li>";
                            }
                        }
                    }
                    sRelated += "</ul>";
                    string sTag = "";
                    //Tags
                    if (objNews.Tags != "")
                    {
                        //panelTag.Visible = true;
                        //ArrayList arrTags = new ArrayList();
                        string[] sTags = objNews.Tags.Split(',');
                        for (int i = 0; i < sTags.Length; i++)
                        {
                            //arrTags.Add(sTags[i]);
                            string tagreplace = sTags[i];
                            tagreplace = tagreplace.Replace(" ", "+");
                            if (sTags[i] != "")
                            {
                                sTag += "<li><a href='/tags.html?tag=" + tagreplace + "'><span class='trending-span'>#</span>" + sTags[i] + "</a><li>";
                            }

                        }
                        //rptTags.DataSource = arrTags;
                        //rptTags.DataBind();
                    }
                    //lay thong tin cong ty
            //        ThongTinCongTyController _Thongtincongty = new ThongTinCongTyController();
            //        ThongTinCongTyInfo objthongtincongty = _Thongtincongty._GetByID(PortalId);
            //        #region Metadata
            //        // Doan nay lam cho the meta
            //        string strmeta = "";
            //        strmeta = "<link rel='stylesheet' href='/static/nvcms/css/jquery.fancybox.min.css'>"
            //            + "<script src='/static/nvcms/js/jquery.fancybox.min.js'></script>"
            //            + "<meta property='og:site_name' content='" + objthongtincongty.TenCongTy + "'/>"
            //            + "<meta property='og:rich_attachment' content='true' />"
            //            + "<meta property='article:publisher' content='https://www.facebook.com/thuongtruong.com.vn/' />"
            //            + "<meta property='og:type' content='article' />"
            //            + "<meta property='og:url' content='__metaturl__' />"
            //            + "<meta property='og:title' content='___metatitle__'/>"
            //            + "<meta property='og:description' content='___metades__'/>"
            //            + "<meta name='description' content='___metades__'/><meta name='tags' content='___TAGs__' /><meta property='og:locale' content='vi_VN' />"
            //            + "<meta property='og:image' content='___metaavatar__'/>"
            //            + "<meta property='og:image:width' content='720' />"
            //            + "__GOOGLEBOOT"
            //            + "<meta http-equiv='Pragma' content='no-cache'> "
            //            + "<meta http-equiv='Expires' content='-1'> "
            //            + "<meta http-equiv='cache-control' content='no-store'> "
            //            + "<meta property='og:image:height' content='378' />"
            //            + "<meta property='article:published_time' content='__createddate__' />"
            //            + "<meta property='article:modified_time' content='__publichddate__' />"
            //            + "<meta property='article:section' content='___section__' />"
            //            + "___ogTAGS___"
            //            + "<meta property='og:tag' content='___TAGs__' />"
            //            + "<meta name='twitter:card' content='___metades__' />"
            //            + "<meta name='twitter:description' content='___metades__' />"
            //            + "<meta name='twitter:title' content='___metatitle__' />"
            //            + "<link rel='alternate' type='application/rss+xml' title='___metatitle__' href='__metaturlfeed__' />"
            //            + "<link rel='alternate' type='application/rss+xml' href='__metaturlfeed2__' />"
            //            + "<meta name='twitter:image' content='___metaavatar__' />"
            //            + "<meta name='news_keywords' content='___TAGs__' />"
            //            + "<meta id='MetaKeywords' name='KEYWORDS' content='___TAGs__' />"
            //            + "<link rel='alternate' href='__metaturl__' hreflang='vi-vn' />"
            //            + "<link rel='canonical' href='__metaturl__' />";
            //        strmeta = strmeta.Replace("___metatitle__", ReplaceChuoi.titlenews(objNews.Title));
            //        strmeta = strmeta.Replace("___metaavatar__", objNews.ImagePath.Replace("/DATA", BL.filesDomain));
            //        strmeta = strmeta.Replace("___metades__", Ultis.SubString(objNews.Summary, 32, ""));
            //        strmeta = strmeta.Replace("__metaturlfeed__", "https://" + Request.Url.Host + "/feednewsdetail.aspx?itemid=" + objNews.NewId);
            //        strmeta = strmeta.Replace("__metaturlfeed2__", "https://" + Request.Url.Host + "/" + Ultis.FormatLinkRSS("sitemaps/rss_feed", objNews.CategoryId, objNews.CategoryName));
            //        strmeta = strmeta.Replace("__metaturlamp__", "https://" + Request.Url.Host + "/amp/" + Ultis.BuildEntryLink(objNews.NewId, objNews.Title.ToLower()));
            //        // requestedUrl = requestedUrl.Replace("http", "https")
            //        strmeta = strmeta.Replace("__metaturl__", requestedUrl);
            //        strmeta = strmeta.Replace("___TAGs__", objNews.Tags);
            //        strmeta = strmeta.Replace("___section__", objNews.CategoryName);
            //        strmeta = strmeta.Replace("__createddate__", objNews.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //        strmeta = strmeta.Replace("__publichddate__", objNews.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //        string sResult1 = "";
            //        if (!string.IsNullOrEmpty(objNews.Tags))
            //        {
            //            string[] strArr = Regex.Split(objNews.Tags, ",");
            //            for (int i = 0; i <= strArr.Count() - 1; i++)
            //                sResult1 += "<meta property=\"article:tag\" content=\"" + ReplaceChuoi.bodau2(strArr[i]) + "\"/>";
            //        }
            //        strmeta = strmeta.Replace("__GOOGLEBOOT", "<meta name='robots' content='index,follow' /><meta name='Googlebot' content='index,follow' /><link rel='amphtml' href='https://" + Request.Url.Host + "/amp/" + Ultis.BuildEntryLink(objNews.NewId, objNews.Title.ToLower()) + ".html'/>");
            //        strmeta = strmeta.Replace("___ogTAGS___", sResult1);
            //        var htmlHeaderTags = "";
            //        LiteralControl htmlHeaderCtrl = new LiteralControl();
            //        htmlHeaderTags = strmeta;
            //        htmlHeaderCtrl.Text = htmlHeaderTags.ToString();
            //        Page.Header.Controls.Add(htmlHeaderCtrl);
            //        // --Boot google
            //        string strgoogle;
            //        strgoogle = "<script type=\"application/ld+json\">"
            //        + "{"
            //        + "\"@context\": \"http://schema.org\","
            //        + "\"@type\": \"NewsArticle\","
            //       + " \"mainEntityOfPage\":{"
            //       + "     \"@type\":\"WebPage\","
            //            + "\"@id\":\"__metaturl__\""
            //        + "},"
            //        + "\"headline\": \"___metatitle__\","
            //        + "\"description\": \"___metades__\","
            //        + "\"image\": {"
            //            + "\"@type\": \"ImageObject\","
            //            + "\"url\": \"___metaavatar__\","
            //         + "   \"width\" : 800,"
            //         + "   \"height\" : 800    },"
            //       + " \"datePublished\": \"___createddate__\","
            //       + " \"dateModified\": \"___createddate2__\","
            //       + " \"author\": {"
            //         + "   \"@type\": \"Person\","
            //         + "   \"name\": \"___uuser__\""
            //       + " },"
            //       + " \"publisher\": {"
            //          + "  \"@type\": \"Organization\","
            //           + " \"name\": \"__Domain__\","
            //           + " \"logo\": {"
            //              + "  \"@type\": \"ImageObject\","
            //              + "  \"url\": \"__Logo___\","
            //              + "  \"width\": 300,"
            //              + "  \"height\": 48"
            //            + "}"
            //        + "}"
            //    + "}"
            //    + "</script>";
            //        string butdanh = "";
            //        if (objNews.ButDanh == "" | objNews.ButDanh == null)
            //        {
            //            butdanh = BL.GetNameByUserId(PortalId, objNews.UserId);
            //        }
                        
            //        else {
            //            butdanh = objNews.ButDanh;
            //        }
                        
            //        strgoogle = strgoogle.Replace("__metaturl__", requestedUrl);
            //        strgoogle = strgoogle.Replace("___metatitle__", ReplaceChuoi.titlenews(objNews.Title));
            //        strgoogle = strgoogle.Replace("___metades__", ReplaceChuoi.titlenews(objNews.Summary));
            //        strgoogle = strgoogle.Replace("___metaavatar__", objNews.ImagePath.Replace("/DATA", BL.filesDomain));
            //        strgoogle = strgoogle.Replace("___uuser__", butdanh);
            //        strgoogle = strgoogle.Replace("___createddate__", objNews.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //        strgoogle = strgoogle.Replace("___createddate2__", objNews.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //        if (objthongtincongty != null)
            //        {
            //            strgoogle = strgoogle.Replace("__Domain__", objthongtincongty.Linkweb);
            //            strgoogle = strgoogle.Replace("__Logo___", objthongtincongty.Logo);
            //        }

            //        var htmlHeaderTags1 = "";
            //        LiteralControl htmlHeaderCtrl1 = new LiteralControl();
            //        htmlHeaderTags1 = strgoogle;
            //        htmlHeaderCtrl1.Text = htmlHeaderTags1.ToString();
            //        Page.Header.Controls.Add(htmlHeaderCtrl1);
            //        string strgoogleindex;
            //        strgoogleindex = "<script type=\"application/ld+json\">"
            //+ "{"
            //    + "\"@context\": \"http://schema.org\","
            //    + "\"@type\": \"BreadcrumbList\","
            //    + "\"itemListElement\": ["
            //        + "{"
            //            + "\"@type\": \"ListItem\","
            //            + "\"position\": 1,"
            //            + "\"item\": {"
            //                + "\"@id\": \"__Domain__\","
            //                + "\"name\": \"Trang chủ\""
            //            + "}"
            //        + "},___StringCAT__"
            //    + "]"
            //+ "}"
            //+ "</script>";

            //        string strgoogleindex1;
            //        strgoogleindex1 = "{"
            //            + "\"@type\": \"ListItem\","
            //            + "\"position\": 2,"
            //            + "\"item\": {"
            //                + "\"@id\": \"__metaturl1__\","
            //                + "\"name\": \"___metatitle__c1\""
            //            + "}"
            //        + "}";

            //        string strgoogleindex2;
            //        strgoogleindex2 = "{"
            //            + "\"@type\": \"ListItem\","
            //            + "\"position\": 3,"
            //            + "\"item\": {"
            //                + "\"@id\": \"__metaturl2__\","
            //                + "\"name\": \"___metatitle__c2\""
            //            + "}"
            //        + "}";


            //        string strgoogleindex4;
            //        strgoogleindex4 = "{"
            //            + "\"@type\": \"ListItem\","
            //            + "\"position\": 4,"
            //            + "\"item\": {"
            //                + "\"@id\": \"__metaturl4__\","
            //                + "\"name\": \"___metatitle__c4\""
            //            + "}"
            //        + "}";
            //        NV_NewsCategoriesController ctcat = new NV_NewsCategoriesController();
            //        NV_NewsCategoriesInfo objCat;
            //        objCat = ctcat.GetByID(objNews.CategoryId);
            //        if (objCat.ParentId == 0)
            //        {
            //            strgoogleindex1 = strgoogleindex1.Replace("__metaturl1__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
            //            strgoogleindex1 = strgoogleindex1.Replace("___metatitle__c1", ReplaceChuoi.titlenews(objCat.CategoryName));
            //            strgoogleindex1 += ",";
            //            strgoogleindex2 = strgoogleindex2.Replace("__metaturl2__", requestedUrl);
            //            strgoogleindex2 = strgoogleindex2.Replace("___metatitle__c2", ReplaceChuoi.titlenews(objNews.Title));
            //            strgoogleindex1 += strgoogleindex2;
            //        }
            //        if (objCat.ParentId > 0)
            //        {
            //            NV_NewsCategoriesInfo objcat2 = ctcat.GetByID(objCat.ParentId);
            //            strgoogleindex1 = strgoogleindex1.Replace("__metaturl1__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objcat2.CategoryID)));
            //            strgoogleindex1 = strgoogleindex1.Replace("___metatitle__c1", ReplaceChuoi.titlenews(objcat2.CategoryName));
            //            strgoogleindex1 += ",";
            //            strgoogleindex2 = strgoogleindex2.Replace("__metaturl2__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
            //            strgoogleindex2 = strgoogleindex2.Replace("___metatitle__c2", ReplaceChuoi.titlenews(objCat.CategoryName));
            //            strgoogleindex2 += ",";
            //            strgoogleindex1 += strgoogleindex2;

            //            strgoogleindex4 = strgoogleindex4.Replace("__metaturl4__", requestedUrl);
            //            strgoogleindex4 = strgoogleindex4.Replace("___metatitle__c4", ReplaceChuoi.titlenews(objNews.Title));
            //            strgoogleindex1 += strgoogleindex4;
            //        }
            //        //lay thong tin cong ty
            //        if (objthongtincongty != null)
            //        {
            //            strgoogleindex = strgoogleindex.Replace("__Domain__", objthongtincongty.Linkweb);
            //        }
            //        //
            //        strgoogleindex = strgoogleindex.Replace("__metaturl__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
            //        strgoogleindex = strgoogleindex.Replace("___metatitle__c", ReplaceChuoi.titlenews(objCat.CategoryName));
            //        strgoogleindex = strgoogleindex.Replace("___StringCAT__", strgoogleindex1);
            //        var htmlHeaderTags2 = "";
            //        LiteralControl htmlHeaderCtrl2 = new LiteralControl();
            //        htmlHeaderTags2 = strgoogleindex;
            //        htmlHeaderCtrl2.Text = htmlHeaderTags2.ToString();
            //        Page.Header.Controls.Add(htmlHeaderCtrl2);
            //        #endregion Metadata
            //        #region crawler



            //        #endregion
                    //Fill du lieu vao template
                    string sTemplate = "";
                    string sTemplateFile = Server.MapPath(PortalSettings.HomeDirectory) + "NewsTemplates/" + setting_details_template;
                    if (File.Exists(sTemplateFile))
                        sTemplate = File.ReadAllText(sTemplateFile);
                    string sContent = "";
                    if (objNews.IsPhoto == true)
                    {
                        sContent += "<p><span class='thongbaoanh'>Click vào ảnh để xem slide</span></p>";
                    }
                    sContent += objNews.Content;
                    if (sContent.Contains(TOKEN_PAGEBREAK))
                    {
                        string[] parttern = new string[] { TOKEN_PAGEBREAK };
                        string[] sPageContent = sContent.Split(parttern, StringSplitOptions.None);
                        string div_page_content = "<ul id='list_page_content'><li>Trang : </li>";
                        string sContentPage = "<div id='list_box_content'><a name='content-page'></a><div id='content_page'></div>";
                        for (int i = 0; i < sPageContent.Length; i++)
                        {
                            string sHide = i > 0 ? "style='display: none'" : "";
                            string sActive = i == 0 ? "class='active'" : "";
                            sContentPage += "<div id='box_content_" + i + "' class='box-content-item' " + sHide + ">" + sPageContent[i] + "</div>";
                            div_page_content += "<li><a id='link_page_" + i + "' " + sActive + " onclick='show_page(" + i + "); return false;' href='#content-page'>" + (i + 1) + "</a></li>";
                        }
                        div_page_content += "</ul>";
                        sContentPage += div_page_content + "</div>";
                        sContent = Server.HtmlDecode(sContentPage);
                    }
                    //LAY THONGT IN USER
                    string usercreate = "";
                    usercreate = BL.GetNameByUserId(PortalId, objNews.UserId);
                    //video
                    
                    string sourceplay = "";
                    if (objNews.IsVideo)
                    {
                        //string playvideo =  "<script src='/static/thixathaihoa/videos/mediaelement-and-player.js'></script>"
                        //+ "<script src='/static/thixathaihoa/videos/renderers/facebook.js'></script>"
                        //+ "<script src='/static/thixathaihoa/videos/demo.js?v=1'></script>";
                        //var htmlHeaderTags2video = "";
                        //LiteralControl htmlHeaderCtrl2video = new LiteralControl();
                        //htmlHeaderTags2video = playvideo;
                        //htmlHeaderCtrl2video.Text = htmlHeaderTags2video.ToString();
                        //Page.Header.Controls.Add(htmlHeaderCtrl2video);
                        if (objNews.SourceText.Contains("facebook"))
                        {
                           sourceplay = "<div class='fb-video' data-href='" + objNews.SourceText + "' data-width='100%' data-show-text='false'></div>";
                           // sourceplay = "<video id='player1' width='640' height='360' style='max-width:100%;' poster='http://www.mediaelementjs.com/images/big_buck_bunny.jpg' preload='none' controls playsinline webkit-playsinline><source src='" + objNews.SourceText + "4' type='video/facebook'><track srclang='en' kind='subtitles' src='mediaelement.vtt'><track srclang='en' kind='chapters' src='chapters.vtt'></video>";
                        }
                    }
                    else
                    {
                        sourceplay = objNews.SourceText;
                    }

                    //Share Social Botton
                    string fbsharelike = "<div class='fb-like' data-size='large' data-href='" + requestedUrl + "'data-layout='button_count' data-action='like' data-size='small' data-show-faces='false' data-share='false'></div>";
                    string fbshare = "<div class='fb-share-button' data-layout='button_count' data-size='large' data-href='" + requestedUrl + "'></div>";
                    sTemplate = sTemplate.Replace(TOKEN_NAME, objNews.Title).Replace(TOKEN_USER, usercreate).Replace(TOKEN_URL, requestedUrl).Replace(TOKEN_NAMETITLE, ReplaceChuoi.titlenews(objNews.Title)).Replace(TOKEN_IMAGE, objNews.ImagePath.Replace("/DATA", BL.filesDomain)).Replace(TOKEN_DATE, BL.FormatDate(objNews.PublishedDate)).Replace(TOKEN_DESCRIPTION, Server.HtmlDecode( objNews.Summary)).Replace(TOKEN_CONTENT, Server.HtmlDecode(sContent)).Replace(TOKEN_RELATED, sRelated)
                        .Replace(TOKEN_SFBlike, fbsharelike).Replace(TOKEN_SFB, fbshare).Replace(TOKEN_TAGS, sTag).Replace(TOKEN_ATTACH, sAttach).Replace(TOKEN_SOURCE, objNews.SourceText).Replace(TOKEN_SOURCEPLAY, sourceplay).Replace(TOKEN_VIEW, Convert.ToString(objNews.ViewCount));
                    ltContent.Text = sTemplate != "" ? sTemplate : setting_details_template + "Module này chưa được áp dụng Template. Vui lòng chọn Template !";

                    //Comment
                    if (setting_enable_comment)
                    {
                        panelComment.Visible = true;
                        BindComments();
                        if (setting_enable_commentLogin)
                        {
                            if (HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                upformcoment.Visible = true;
                                updateloingcoment.Visible = false;
                                txtName.Text = BL.GetNameByUserId(PortalId, UserId);
                                txtEmail.Text =UserController.Instance.GetCurrentUserInfo().Email;
                                txtName.Enabled = false;
                                txtEmail.Enabled = false;
                            }
                            else
                            {
                                upformcoment.Visible = false;
                                updateloingcoment.Visible = true;
                            }
                        }
                        else
                        {
                            
                            upformcoment.Visible = true;
                        }
                    }
                    //if (setting_enable_other==false)
                    //{
                    //    paneOther.Visible = false;
                    //}

                    //newsController.IncrementViewCount(ItemID);
                    //Tin khac cung chuyen muc
                    if (setting_details_more > 0)
                    {
                        //vbRelated.TotalItem = setting_details_more;
                        //vbRelated.ItemID = ItemID;
                        //vbRelated.Title = "Tin khác";
                        //if (setting_type == "Scroll")
                        //{
                        //    vbRelated.scroll = true;
                        //    vbRelated.scrollpage = setting_details_morepage;
                        //    vbRelated.setting_imgWidth = setting_imgWidth;
                        //    vbRelated.setting_imgHeight = setting_imgHeight;
                        //}
                        //else
                        //{

                        //}

                    }

                    // Luu ve tu URL ngoai
                    if (Request != null)
                    {
                        var host = Request.Url.Host;
                        if (Request.UrlReferrer != null)
                        {
                            var refererUrl = Request.UrlReferrer.ToString();
                            if (refererUrl.Contains(host))
                            {
                            }
                            else
                            {
                                NewsByShareController ctlnewbyshare = new NewsByShareController();
                                ctlnewbyshare._Insert(ItemID, refererUrl, DateTime.Now);
                            }
                        }
                    }
                    //So luot xem
                    string soluotxem = "soluotxem:" + objNews.NewId;
                    if (Session[soluotxem] != null)
                    {
                    }
                    else
                    {
                        Session[soluotxem] = soluotxem;
                        NewsByView ctlnewsbyView = new NewsByView();
                        ctlnewsbyView.NewsByView_Update(objNews.NewId);
                    }
                }
                // ltContent.Text = "details";
            }
            }
            catch (Exception ex) { ltContent.Text = ex.Message; }
        }

        private void BindComments()
        {
            if (Request.QueryString["page"] != null) CurrentPage = Convert.ToInt32(Request.QueryString["page"]);

            NV_NewsFeedbackController controller = new NV_NewsFeedbackController();
            int iCount = controller.GetByNewsID_Count(ItemID, Status);
            lbliCount.Text = iCount.ToString();
            if (iCount % PageSize != 0)
                vbPaging.TotalPage = iCount / PageSize + 1;
            else vbPaging.TotalPage = iCount / PageSize;

            if (vbPaging.TotalPage <= 1)
            {
                vbPaging.Visible = false;
            }
            else
            {
                vbPaging.bindPages();
                vbPaging.Visible = true;
            }
            var arrayList = controller.GetByNewsID_Index(ItemID, Status, CurrentPage, PageSize);
            if (arrayList == null) arrayList = new System.Collections.ArrayList();
            rptComment.DataSource = arrayList;
            rptComment.DataBind();
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
        protected void linkSendComment_Click(object sender, EventArgs e)
        {
            try
            {
                if (ItemID > 0 && ctlCaptcha.IsValid)
                {
                    var ctl = new NV_NewsFeedbackController();
                    var info = new NV_NewsFeedbackInfo();
                    info.NewsId = ItemID;
                    info.Title = FillterInput(txtTitle.Text);
                    info.Email = FillterInput(txtEmail.Text);
                    info.FullName = FillterInput(txtName.Text);
                    info.Content = FillterInput(txtContent.Text);
                    info.Status = Convert.ToInt32(CommentStatus.Created);
                    info.CreateDate = DateTime.Now;
                    info.IPTrack = Request.ServerVariables["REMOTE_ADDR"];

                    ctl.Insert(info);
                    ClientAPI.RegisterStartUpScript(this.Page, "showSuccess", "<script>alert('Gửi bình luận thành công!');closeFrom();</script>");
                    System.Threading.Thread.Sleep(500);
                }
                else
                    ClientAPI.RegisterStartUpScript(this.Page, "showError", "<script>alert('Gửi bình luận thất bại!');</script>");
            }
            catch { }
        }


    }
}