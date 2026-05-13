using DotNetNuke.Entities.Modules;
using DotNetNuke;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NVCMS.Modules.TinTuc;
using System.Collections;
using HtmlAgilityPack;
using DotNetNuke.Entities.Portals;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using System.IO;
using System.Web.Caching;
using System.Web.Hosting;

public partial class feeds : System.Web.UI.Page

{
    string strLit;
    string strUrl;
    string UrlAlias = "";
    string strImg;
    int itemid = 0;
    NV_NewsController ctl = new NV_NewsController();
    ArrayList arr = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ltrmenusidebar.Text = GetTopMenu();
            string sUrl1 = Request.RawUrl;
            itemid = Ultis.GetRequestId(sUrl1);
            NV_NewsInfo objNews = ctl.GetByID(itemid);
            if (objNews != null)
            {
                if (objNews.Status != 2)
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId));
                }
                if (objNews.isActive == false)
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId));
                }
                if (objNews.IsAnNoiDung == true)
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId));
                }
                #region Metadata
                this.Page.Title = objNews.Title;
                //tags
                string sTagMeta = "";

                if (objNews.Tags != "")
                {

                    string[] sTags = objNews.Tags.Split(',');
                    for (int i = 0; i < sTags.Length; i++)
                    {
                        string tagreplace = sTags[i];
                        tagreplace = tagreplace.Replace(" ", "-");
                        sTagMeta += sTags[i] + ",";
                    }
                }
                //og image

                HtmlMeta ogimage = new HtmlMeta();
                ogimage.Attributes.Add("property", "og:image");
                ogimage.Content = objNews.ImagePath.Replace("/DATA", BL.filesDomain);
                this.Page.Header.Controls.Add(ogimage);

                HtmlMeta meta12222 = new HtmlMeta();
                meta12222.Attributes.Add("property", "og:title");
                meta12222.Content = objNews.Title;
                this.Page.Header.Controls.Add(meta12222);

                HtmlMeta meta12222a = new HtmlMeta();
                meta12222a.Attributes.Add("property", "og:description");
                meta12222a.Content = objNews.Summary;
                this.Page.Header.Controls.Add(meta12222a);

                HtmlMeta meta11 = new HtmlMeta();
                meta11 = new HtmlMeta();
                meta11.Attributes.Add("property", "og:site_name");
                meta11.Content = "Capstone Vietnam";
                this.Page.Header.Controls.Add(meta11);

                HtmlMeta meta12 = new HtmlMeta();
                meta12 = new HtmlMeta();
                meta12.Attributes.Add("property", "og:locale");
                meta12.Content = "vi-VN";
                this.Page.Header.Controls.Add(meta12);

                HtmlMeta meta2 = new HtmlMeta();
                meta2.Attributes.Add("property", "og:tags");
                meta2.Content = sTagMeta;
                this.Page.Header.Controls.Add(meta2);

                HtmlMeta meta3 = new HtmlMeta();
                meta3.Attributes.Add("property", "og:type");
                meta3.Content = "article";
                this.Page.Header.Controls.Add(meta3);

                HtmlMeta meta4 = new HtmlMeta();
                string requestedUrl = (string)HttpContext.Current.Items["UrlRewrite:OriginalUrl"];
                meta4.Attributes.Add("property", "og:url");
                meta4.Content = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title);
                this.Page.Header.Controls.Add(meta4);

                HtmlMeta meta5 = new HtmlMeta();
                meta5.Attributes.Add("property", "twitter:card");
                meta5.Content = objNews.Summary;
                this.Page.Header.Controls.Add(meta5);

                HtmlMeta meta6 = new HtmlMeta();
                meta6.Attributes.Add("property", "twitter:title");
                meta6.Content = objNews.Title;
                this.Page.Header.Controls.Add(meta6);

                HtmlMeta meta7 = new HtmlMeta();
                meta7.Attributes.Add("property", "twitter:image");
                meta7.Content = objNews.ImagePath.Replace("/DATA", BL.filesDomain);
                this.Page.Header.Controls.Add(meta7);

                HtmlMeta meta8 = new HtmlMeta();
                meta8.Name = "ROBOTS";
                meta8.Content = "index,follow";
                this.Page.Header.Controls.Add(meta8);

                HtmlMeta meta81 = new HtmlMeta();
                meta81.HttpEquiv = "content-language";
                meta81.Content = "vi";
                this.Page.Header.Controls.Add(meta81);

                HtmlMeta meta81a = new HtmlMeta();
                meta81a.HttpEquiv = "content-language";
                meta81a.Content = "text/html; charset=UTF-8";
                this.Page.Header.Controls.Add(meta81a);

                HtmlMeta meta122223x = new HtmlMeta();
                meta122223x.Attributes.Add("name", "googlebot");
                meta122223x.Content = ("index, follow");
                this.Page.Header.Controls.Add(meta122223x);

                //HtmlMeta metarobots = new HtmlMeta();
                //metarobots.Attributes.Add("name", "robots");
                //metarobots.Content = ("noarchive");
                //cp.Header.Controls.Add(metarobots);
                //--Boot google

                {
                    string strgoogle = null;
                    //<link rel=\"canonical\" href=\"____canonical_____\" />
                    strgoogle = "<link rel='canonical' href='____canonical_____' /><link rel='alternate' type='application/rss+xml' title='___metatitle__' href='__metaturlfeed__' /><script type=\"application/ld+json\">{\"@context\": \"http://schema.org\",\"@type\": \"NewsArticle\", \"mainEntityOfPage\":{     \"@type\":\"WebPage\",\"@id\":\"__metaturl__\"},\"headline\": \"___metatitle__\",\"description\": \"___metades__\",\"image\": {\"@type\": \"ImageObject\",\"url\": \"___metaavatar__\",   \"width\" : 800,   \"height\" : 800    }, \"datePublished\": \"___createddate__\", \"dateModified\": \"___createddate2__\", \"author\": {   \"@type\": \"Person\",   \"name\": \"___uuser__\" }, \"publisher\": {  \"@type\": \"Organization\", \"name\": \"___link____\", \"logo\": {  \"@type\": \"ImageObject\",  \"url\": \"https://cdn.thuongtruong.com.vn//thuongtruong/images/logo.png?v=2.3.1\",  \"width\": 300,  \"height\": 48}}}</script>";
                    strgoogle = strgoogle.Replace("____canonical_____", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title));
                    strgoogle = strgoogle.Replace("__metaturl__", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title));
                    strgoogle = strgoogle.Replace("___metatitle__", objNews.Title.Replace("\"", ""));
                    strgoogle = strgoogle.Replace("___metades__", objNews.Summary.Replace("\"", ""));
                    strgoogle = strgoogle.Replace("___metaavatar__", objNews.ImagePath.Replace("/DATA", BL.filesDomain));
                    strgoogle = strgoogle.Replace("___uuser__", BL.GetNameByUserId(PortalController.GetCurrentPortalSettings().PortalId, objNews.UserId));
                    strgoogle = strgoogle.Replace("___createddate__", objNews.CreateDate.ToString("yyyy-MM-ddThh:mm:sszzz"));
                    strgoogle = strgoogle.Replace("___createddate2__", objNews.PublishedDate.ToString("yyyy-MM-ddThh:mm:sszzz"));
                    strgoogle = strgoogle.Replace("___metatitle__", objNews.Title.Replace("\"", ""));
                    strgoogle = strgoogle.Replace("__metaturlfeed__", "http://" + Request.Url.Host + "/feednewsdetail.aspx?itemid=" + objNews.NewId);
                    strgoogle = strgoogle.Replace("___link____", Request.Url.Host);

                    dynamic htmlHeaderTags1 = "";
                    LiteralControl htmlHeaderCtrl1 = new LiteralControl();
                    htmlHeaderTags1 = strgoogle;
                    htmlHeaderCtrl1.Text = htmlHeaderTags1.ToString();
                    Page.Header.Controls.Add(htmlHeaderCtrl1);
                    string strgoogleindex = null;
                    strgoogleindex = "<script type=\"application/ld+json\">{\"@context\": \"http://schema.org\",\"@type\": \"BreadcrumbList\",\"itemListElement\": [{\"@type\": \"ListItem\",\"position\": 1,\"item\": {\"@id\": \"___link____\",\"name\": \"Trang chủ\"}},___StringCAT__]}</script>";

                    string strgoogleindex1 = null;
                    strgoogleindex1 = "{\"@type\": \"ListItem\",\"position\": 2,\"item\": {\"@id\": \"__metaturl1__\",\"name\": \"___metatitle__c1\"}}";

                    string strgoogleindex2 = null;
                    strgoogleindex2 = "{\"@type\": \"ListItem\",\"position\": 3,\"item\": {\"@id\": \"__metaturl2__\",\"name\": \"___metatitle__c2\"}}";
                    NV_NewsCategoriesController ctcat = new NV_NewsCategoriesController();
                    NV_NewsCategoriesInfo objCat = default(NV_NewsCategoriesInfo);
                    objCat = ctcat.GetByID(objNews.CategoryId);
                    if (objCat.ParentId == 0)
                    {
                        strgoogleindex1 = strgoogleindex1.Replace("__metaturl1__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
                        strgoogleindex1 = strgoogleindex1.Replace("___metatitle__c1", objCat.CategoryName);
                    }
                    if (objCat.ParentId > 0)
                    {
                        NV_NewsCategoriesInfo objcat2 = ctcat.GetByID(objCat.ParentId);
                        strgoogleindex1 = strgoogleindex1.Replace("__metaturl1__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objcat2.CategoryID)));
                        strgoogleindex1 = strgoogleindex1.Replace("___metatitle__c1", objcat2.CategoryName);
                        strgoogleindex1 += ",";
                        strgoogleindex2 = strgoogleindex2.Replace("__metaturl2__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
                        strgoogleindex2 = strgoogleindex2.Replace("___metatitle__c2", objCat.CategoryName);
                        strgoogleindex1 += strgoogleindex2;
                        //If objcat2.ParentId > 0 Then
                        //    Response.Write(objcat2.CategoryID & "3")
                        //End If
                    }
                    strgoogleindex = strgoogleindex.Replace("__metaturl__", DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objCat.CategoryID)));
                    strgoogleindex = strgoogleindex.Replace("___metatitle__c", objCat.CategoryName);
                    strgoogleindex = strgoogleindex.Replace("___StringCAT__", strgoogleindex1);
                    strgoogleindex = strgoogleindex.Replace("___link____", Request.Url.Host);
                    dynamic htmlHeaderTags2 = "";
                    LiteralControl htmlHeaderCtrl2 = new LiteralControl();
                    htmlHeaderTags2 = strgoogleindex;
                    htmlHeaderCtrl2.Text = htmlHeaderTags2.ToString();
                    Page.Header.Controls.Add(htmlHeaderCtrl2);
                }

                #endregion Metadata
                string strgoooglean = null;
                strgoooglean = "<script type=\"application/json\">{\"vars\": { \"account\": \"UA-112829750-1\" },\"triggers\": { \"trackPageviewWithCustomUrl\": { \"on\": \"visible\", \"request\": \"pageview\", \"vars\": { \"title\": \"___TITLTEEE___\", \"documentLocation\": \"___TITLTEEEURL___\" }}}}</script>";
                strgoooglean = strgoooglean.Replace("___TITLTEEE___", objNews.Title);
                strgoooglean = strgoooglean.Replace("___TITLTEEEURL___", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title));
                ltrga.Text = strgoooglean;
                #region analystic

                #endregion
                //avatar
                //ltravatar.Text = "<amp-img layout='responsive' src='" + objNews.ImagePath + "' width='690' height='388'></amp-img>";

                ltrcat.Text = "<a href='" + DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objNews.CategoryId)) + "' data-wpel-link='internal'>" + objNews.CategoryName + "</a>";
                lbltilte.Text = objNews.Title;
                ltrcreatedate.Text = BL.FormatDate(objNews.PublishedDate);
                ltrchitietbaititle.Text = "<a href='" + Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title) + "' data-wpel-link='internal'>" + objNews.Title + "</a>";
                ltrchitietbaicat.Text = "<a href='" + DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(objNews.CategoryId)) + "' data-wpel-link='internal'>" + objNews.CategoryName + "</a>";
                if (objNews.ButDanh != "")
                {
                    ltrbutdanh.Text = objNews.ButDanh;
                }
                else
                {
                    ltrbutdanh.Text = BL.GetButDanh(0, objNews.UserId);
                }
                if (objNews.IsAnNoiDung == true)
                {
                    ltrsumary.Text = "";
                    ltrcontent.Text = "<div style='text-align: center; padding: 10px;'><amp-img width='150px' height='150px' src='http://f.thuongtruong.com.vn/loading.gif' title='Đang tải nội dung'></amp-img><p><h3>Đang tải nội dung</h3></div>";
                }
                else
                {
                    ltrsumary.Text = objNews.Summary;
                    ltrcontent.Text = ConvertContentToAMP(objNews.Content);
                    //tag
                    strUrl = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title);
                    string sTag = "";
                    //Tags
                    if (objNews.Tags != "")
                    {
                        //panelTag.Visible = true;
                        //ArrayList arrTags = new ArrayList();
                        string[] sTags = objNews.Tags.Split(',');
                        ArrayList arrtaga = new ArrayList();
                        for (int i = 0; i < sTags.Length; i++)
                        {
                            //arrTags.Add(sTags[i]);
                            string tagreplace = sTags[i];
                            tagreplace = tagreplace.Replace(" ", "-");
                            sTag += "<a href='/tags.html?tag=" + tagreplace + "'>#" + sTags[i] + "</a>";
                            arrtaga.Insert(i, sTags[i]);
                        }
                        rptContenttags.DataSource = arrtaga;
                        rptContenttags.DataBind();
                    }
                    ltrtag.Text = sTag;
                    ////share
                    //string requestedUrl2 = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objNews.CategoryId), objNews.NewId, objNews.Title);
                    //string twtiiter = "<li><a class='s_fb' target='_blank' href='https://www.facebook.com/sharer.php?u=" + requestedUrl2 + "&amp;text=" + objNews.Title + "' data-wpel-link='internal'></a></li><li><a class='s_tw' target='_blank' href='https://twitter.com/intent/tweet?url=" + requestedUrl2 + "&amp;text=" + objNews.Title + "' data-wpel-link='internal'></a></li><li><a class='s_gp' target='_blank' href='https://plus.google.com/share?url=" + requestedUrl2 + "' data-wpel-link='internal'></a></li><li><a class='s_lk' target='_blank' href='https://www.linkedin.com/shareArticle?url=" + requestedUrl2 + "&amp;title=Top 7 điều kiện xin học bổng du học mỹ 2018 mà bạn chưa biết' data-wpel-link='internal'></a></li><li><a class='s_wp' target='_blank' href='whatsapp://send?text=" + requestedUrl2 + "' data-action='share/whatsapp/share' data-wpel-link='internal'></a></li>";

                    //ltrshare.Text = twtiiter;
                    //Lay tin lien quan
                    var listMore = ctl.SelectIndex(itemid.ToString(), objNews.CategoryId, 0, 1, 6, "", false);
                    rptlienquan.DataSource = listMore;
                    rptlienquan.DataBind();
                    //Moi nhat
                    //var listmoi = ctl.selectnewsinsamecat(itemid, objNews.CategoryId, 6);
                    var listmoi = ctl.ShowBaiMoiNhat("", 0, 20);
                    //Response.Write(PortalController.GetCurrentPortalSettings().PortalId);
                    rptmoinhat.DataSource = listmoi;
                    rptmoinhat.DataBind();
                }

            }
            else
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId));
            }

        }
    }
    protected void rptContenttagsItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string CatId = (e.Item.FindControl("tagid") as Label).Text;
            NewsByTagsController ctl = new NewsByTagsController();
            ArrayList arrHots = ctl.NewsByTags_GetByTags("", ReplaceChuoi.bodau2(CatId), 4);
            if (arrHots.Count > 3)
            {
                Repeater rptListNewsCatHot = e.Item.FindControl("rptListNewsCatHot") as Repeater;
                rptListNewsCatHot.DataSource = arrHots;
                rptListNewsCatHot.DataBind();
            }
        }
    }
    public bool Checkhien(string NewId)
    {
        NewsByTagsController ctl = new NewsByTagsController();
        ArrayList arrHots = ctl.NewsByTags_GetByTags("", ReplaceChuoi.bodau2(NewId), 1);
        if (arrHots.Count < 4)
            return false;
        else
            return true;
    }
    public static string ConvertContentToAMP(string inputHtml)
    {
        try
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(inputHtml);
            HtmlNode node;

            #region remove colgroup tag
            while ((node = doc.DocumentNode.SelectSingleNode("//colgroup")) != null)
            {
                node.Remove();
            }
            #endregion

            #region Image
            var listImageNode = doc.DocumentNode.SelectNodes("//img");
            if (listImageNode != null)
            {
                foreach (var imageNode in listImageNode)
                {
                    //old
                    var parentNode = imageNode.ParentNode;
                    var imageCommentNode = imageNode.NextSibling;
                    if (parentNode != null)
                    {
                        //new
                        var ampParentNode = doc.CreateElement("figure");
                        var ampImageNode = doc.CreateElement("amp-img");
                        var ampImageCommentNode = doc.CreateElement("figcaption");

                        //change image node
                        var currentImageAttributes = imageNode.Attributes;

                        if (currentImageAttributes.Count > 0)
                        {
                            List<string> listAttributesRemove = new List<string>();
                            //get list att remove
                            foreach (var attributes in currentImageAttributes)
                            {
                                if (attributes.Name != "alt" && attributes.Name != "src" && attributes.Name != "width" && attributes.Name != "height")
                                {
                                    listAttributesRemove.Add(attributes.Name);
                                }
                            }
                            //remove att not use
                            if (listAttributesRemove.Count > 0)
                            {
                                foreach (var item in listAttributesRemove)
                                {
                                    imageNode.Attributes[item].Remove();
                                }
                            }
                            //set att to amp image
                            foreach (var attributes in currentImageAttributes)
                            {
                                ampImageNode.SetAttributeValue(attributes.Name, attributes.Value);
                            }
                        }

                        //change image comment node
                        if (imageCommentNode != null)
                        {
                            if (imageCommentNode.Name != "amp-img")
                            {
                                ampImageCommentNode.InnerHtml = imageCommentNode.InnerHtml;
                                imageCommentNode.Remove();
                            }
                        }

                        //append element
                        imageNode.ParentNode.InsertAfter(ampParentNode, imageNode);
                        imageNode.Remove();
                        //add layout att to amp image
                        ampImageNode.SetAttributeValue("layout", "responsive");
                        ampImageNode.SetAttributeValue("class", "amp-wp-enforced-sizes");
                        ampImageNode.SetAttributeValue("width", "540");
                        ampImageNode.SetAttributeValue("height", "310");
                        ampParentNode.AppendChild(ampImageNode);
                        ampParentNode.AppendChild(ampImageCommentNode);

                    }

                }
            }
            #endregion
            #region Video
            var listVideoNode = doc.DocumentNode.SelectNodes("//video");

            if (listVideoNode != null)
            {
                foreach (var VideoNode in listVideoNode)
                {
                    var VideoLink = "";
                    if (VideoNode.Attributes["src"] != null)
                    {
                        VideoLink = VideoNode.Attributes["src"].Value;
                    }

                    //Video youtube

                    var ampVideoNode = doc.CreateElement("amp-video");

                    ampVideoNode.SetAttributeValue("src", VideoLink);
                    ampVideoNode.SetAttributeValue("layout", "responsive");

                    if (VideoNode.Attributes["width"] != null)
                    {
                        ampVideoNode.SetAttributeValue("width", VideoNode.Attributes["width"].Value);
                    }
                    else
                    {
                        ampVideoNode.SetAttributeValue("width", "400");
                    }
                    if (VideoNode.Attributes["height"] != null)
                    {
                        ampVideoNode.SetAttributeValue("height", VideoNode.Attributes["height"].Value);
                    }
                    else
                    {
                        ampVideoNode.SetAttributeValue("height", "300");
                    }
                    if (VideoNode.Attributes["controls"] != null)
                    {
                        ampVideoNode.SetAttributeValue("controls", "");
                    }
                    else
                    {
                        ampVideoNode.SetAttributeValue("controls", "");
                    }
                    VideoNode.ParentNode.InsertAfter(ampVideoNode, VideoNode);
                    VideoNode.Remove();


                }
            }
            #endregion
            #region Iframe
            var listIframeNode = doc.DocumentNode.SelectNodes("//iframe");

            if (listIframeNode != null)
            {
                foreach (var iframeNode in listIframeNode)
                {
                    var iframeLink = "";
                    if (iframeNode.Attributes["src"] != null)
                    {
                        iframeLink = iframeNode.Attributes["src"].Value;
                    }

                    //iframe youtube
                    if (iframeLink.Contains("youtube.com"))
                    {
                        string youtubeID = "";
                        var ampYoutubeNode = doc.CreateElement("amp-youtube");
                        var regexV = Regex.Match(iframeLink, "youtube.com/v/").Captures;
                        if (regexV.Count > 0)
                        {
                            youtubeID = iframeLink.Substring(iframeLink.LastIndexOf("youtube.com/v/")).Replace("youtube.com/v/", "");
                        }
                        else
                        {
                            var regexEmbed = Regex.Match(iframeLink, "youtube.com/embed/").Captures;
                            if (regexEmbed.Count > 0)
                            {
                                youtubeID = iframeLink.Substring(iframeLink.LastIndexOf("youtube.com/embed/")).Replace("youtube.com/embed/", "");
                            }
                        }

                        if (youtubeID.Contains("?"))
                        {
                            youtubeID = youtubeID.Substring(0, youtubeID.LastIndexOf("?")).Replace("?", "");
                        }

                        ampYoutubeNode.SetAttributeValue("data-videoid", youtubeID);
                        ampYoutubeNode.SetAttributeValue("layout", "responsive");
                        if (iframeNode.Attributes["width"] != null)
                        {
                            ampYoutubeNode.SetAttributeValue("width", iframeNode.Attributes["width"].Value);
                        }
                        else
                        {
                            ampYoutubeNode.SetAttributeValue("width", "0");
                        }
                        if (iframeNode.Attributes["height"] != null)
                        {
                            ampYoutubeNode.SetAttributeValue("height", iframeNode.Attributes["height"].Value);
                        }
                        else
                        {
                            ampYoutubeNode.SetAttributeValue("height", "0");
                        }

                        iframeNode.ParentNode.InsertAfter(ampYoutubeNode, iframeNode);
                        iframeNode.Remove();
                    }
                    //orther iframe
                    else
                    {
                        var ampIframeNode = doc.CreateElement("amp-iframe");

                        ampIframeNode.SetAttributeValue("src", iframeLink);
                        ampIframeNode.SetAttributeValue("layout", "responsive");
                        ampIframeNode.SetAttributeValue("sandbox", "allow-scripts allow-same-origin allow-popups");
                        ampIframeNode.SetAttributeValue("allowfullscreen", "");
                        ampIframeNode.SetAttributeValue("frameborder", "0");

                        if (iframeNode.Attributes["width"] != null)
                        {
                            ampIframeNode.SetAttributeValue("width", iframeNode.Attributes["width"].Value);
                        }
                        else
                        {
                            ampIframeNode.SetAttributeValue("width", "0");
                        }
                        if (iframeNode.Attributes["height"] != null)
                        {
                            ampIframeNode.SetAttributeValue("height", iframeNode.Attributes["height"].Value);
                        }
                        else
                        {
                            ampIframeNode.SetAttributeValue("height", "0");
                        }

                        iframeNode.ParentNode.InsertAfter(ampIframeNode, iframeNode);
                        iframeNode.Remove();

                    }
                }
            }
            #endregion

            #region remove attributes not use
            while ((node = doc.DocumentNode.SelectSingleNode("//@target")) != null)
            {
                node.Attributes["target"].Remove();
            }
            //remove <p><br></p>
            var listPNode = doc.DocumentNode.SelectNodes("//p");
            if (listPNode != null)
            {
                foreach (var pNode in listPNode)
                {
                    var childNode = pNode.ChildNodes;
                    if (childNode.Count == 1)
                    {
                        var firstChild = childNode.FirstOrDefault();
                        if (firstChild.Name == "br")
                        {
                            pNode.Remove();
                        }
                    }
                }
            }
            //remove <br>
            while ((node = doc.DocumentNode.SelectSingleNode("//br")) != null)
            {
                node.Remove();
            }
            #endregion


            string outputHtml = doc.DocumentNode.OuterHtml;

            outputHtml = new Regex("/(\n)+/g").Replace(outputHtml, "");

            return outputHtml;
        }
        catch (Exception ex)
        {
            //log.ErrorFormat("Message: {0} - Action: {1} - Error: {2}", ex.Message, "ChangeHTML_ConvertContentAMP", ex);
            return inputHtml;
        }
    }
    //public string GetTopMenu()
    //{
    //    if (File.Exists(@"/Portals/47/MenusXML/MainMenu.xml"))
    //    {
    //        // Lấy đường dẫn file menu
    //        //PortalSettings.HomeDirectory
    //        string mXmlPath = @"/Portals/47/MenusXML/MainMenu.xml";

    //        // Đọc file
    //        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
    //        xmlDoc.Load(Server.MapPath(mXmlPath));
    //        XmlElement objRoot = xmlDoc.DocumentElement;
    //        // Tạo menu
    //        string name = string.Empty;
    //        string link = string.Empty;
    //        string mnid = string.Empty;
    //        int iTabid = Null.NullInteger;
    //        string sParam = string.Empty;
    //        string iTaget = string.Empty;
    //        StringBuilder sbMenu = new StringBuilder();
    //        XmlNodeList arrMainCateInfo = objRoot.ChildNodes;
    //        foreach (XmlNode objMenusInfo in arrMainCateInfo)
    //        {
    //            name = objMenusInfo.Attributes["Text"].Value;
    //            link = objMenusInfo.Attributes["NavUrl"].Value;
    //            mnid = objMenusInfo.Attributes["Value"].Value;
    //            iTabid = int.Parse(objMenusInfo.Attributes["TabId"].Value);
    //            iTaget = objMenusInfo.Attributes["Target"].Value;
    //            // Dim mainMenu As New mainMenu(name, link, mnid, iTabid, sParam)
    //            if (objMenusInfo.ChildNodes.Count == 0)
    //                sbMenu.AppendFormat("<div class='sidebar-menu'><a href='{0}' class='sidebar-item'>{1}<i class='fa fa-circle'></i></a></div>", link, name);
    //            else
    //            {
    //                sbMenu.AppendFormat("<amp-accordion class='sidebar-menu'><section>");
    //                sbMenu.AppendFormat("<h4>{0}<i class='fa fa-angle-down'></i></h4>", name);
    //                sbMenu.AppendFormat("<ul>");
    //                // Cap 2
    //                XmlNodeList arrSubCateInfo = objMenusInfo.ChildNodes;
    //                for (int i = 0; i <= arrSubCateInfo.Count - 1; i++)
    //                {
    //                    XmlNode objAISubMenuInfo = arrSubCateInfo[i];

    //                    name = objAISubMenuInfo.Attributes["Text"].Value;
    //                    link = objAISubMenuInfo.Attributes["NavUrl"].Value;
    //                    mnid = objMenusInfo.Attributes["Value"].Value;
    //                    iTabid = int.Parse(objAISubMenuInfo.Attributes["TabId"].Value);
    //                    sbMenu.AppendFormat("<li><a href='{0}'><i class='fa fa-angle-right'></i>{1}<i class='fa fa-circle'></i></a></li>", link, name);
    //                }
    //                sbMenu.AppendFormat("</ul></section></amp-accordion>");
    //            }
    //        }
    //        return sbMenu.ToString();

    //    }
    //    else
    //    {
    //        return "d";  //Directory.GetFiles(Server.MapPath("~/Portals/47/MenusXML/MainMenu.xml")).ToString();

    //    }
    //}
}


