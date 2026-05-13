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
public partial class feeds : System.Web.UI.Page
{
    string strLit;
    string strUrl;
    string UrlAlias = "";
    string strImg;
    NV_NewsController ctl = new NV_NewsController();
    ArrayList arr = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            FeedRss rss = new FeedRss();
            FeedRss.RssChannel channel = new FeedRss.RssChannel();
            channel.Title = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalName;
            channel.Link = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalAlias.HTTPAlias.ToString();
            channel.Description = "ThuongTruong - RSS Results	only for asset type(s) of article";
            rss.AddRssChannel(channel);
            arr = ctl.ShowSelectIndex("", 0, 0, "vi-VN", 1, 10, false);
            foreach (NV_NewsInfo obj in arr)
            {
                FeedRss.RssItem item = new FeedRss.RssItem();
                strImg = obj.ImagePath;
                strUrl = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title);
                item.Title = obj.Title;
                item.CreateDate = obj.PublishedDate.ToString();
                item.Link = strUrl;
                item.Description = obj.Summary;
                item.guid = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title);
                string conntetnnew = "";
                conntetnnew = obj.Content;
                conntetnnew = conntetnnew.Replace("<h3", "<h1");
                conntetnnew = conntetnnew.Replace("<h2", "<h1");
                conntetnnew = conntetnnew.Replace("</h3>", "</h1>");
                conntetnnew = conntetnnew.Replace("</h2>", "</h1>");
                conntetnnew = conntetnnew.Replace("<p>&nbsp</p>", "");
                conntetnnew = conntetnnew.Replace("/DATA", BL.filesDomain);
                conntetnnew = CleanStyle(conntetnnew);
                //conntetnnew = ConvertContentToAMP(conntetnnew);
                conntetnnew = ConvertContentToFBInstant(conntetnnew);
                string strcontent = "<!doctype html><html lang='en' prefix='op: http://media.facebook.com/op#'><head><meta charset='utf-8'><link rel='canonical' href='____LINK____'><meta property='op:markup_version' content='v1.0'><meta property='fb:use_automatic_ad_placement' content='false'></head><body><article><header><h1>____TITLE____</h1><time class='op-published' datetime='____PUBLICKDATE____'>____PUBLICKDATE____</time><time class='op-modified' dateTime='____CREATEDATE____'>____CREATEDATE____</time><figure data-feedback='fb:likes, fb:comments'><img src='____AVATAR____' /></figure></header>____CONTENT____<iframe><!-- Google Tag Manager --><script type='text/javascript'>var _gaq = _gaq || [];_gaq.push(['_setAccount', 'UA-27937229-1']);_gaq.push(['_trackPageview']);(function() {var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s); })();</script></iframe><footer><small>&copy; 2017 TẠP CHÍ THƯƠNG TRƯỜNG</small></footer></article></body></html>";
                strcontent = strcontent.Replace("____LINK____", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title));
                strcontent = strcontent.Replace("____TITLE____", obj.Title);
                strcontent = strcontent.Replace("____PUBLICKDATE____", obj.PublishedDate.ToString("s"));
                strcontent = strcontent.Replace("____CREATEDATE____", obj.CreateDate.ToString("s"));
                strcontent = strcontent.Replace("____AVATAR____", obj.ImagePath.Replace("/DATA", BL.filesDomain));
                strcontent = strcontent.Replace("____CONTENT____", conntetnnew);
                strcontent = strcontent.Replace("\"", "'");
                strcontent = strcontent.Replace("<p id='AdAsia'>", "");
                strcontent = strcontent.Replace("<p><figure>", "<div><figure>");
                strcontent = strcontent.Replace("</figure></p>", "</figure></div>");
                item.contentendcodeElement = "<![CDATA[" + strcontent + "]]>";
                rss.AddRssItem(item);
            }

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write(rss.RssDocument);
            Response.End();
        }
    }
    public static string CleanStyle(string inputHtml)
    {
        try
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(inputHtml);
            HtmlNode node;

            //while ((node = doc.DocumentNode.SelectSingleNode("//a")) != null)
            //{
            //    var span = doc.CreateElement("b");
            //    span.InnerHtml = node.InnerHtml;
            //    node.ParentNode.InsertAfter(span, node);
            //    node.Remove();
            //}
            #region Table
            var listTableNode = doc.DocumentNode.SelectNodes("//table");
            if (listTableNode != null)
            {
                foreach (var tableNode in listTableNode)
                {
                    //remove att of table
                    var currentTableAttributes = tableNode.Attributes;
                    List<string> listAttributesRemove = new List<string>();
                    //get list att remove
                    foreach (var attributes in currentTableAttributes)
                    {
                        if (attributes.Name != "class" && attributes.Name != "colspan" && attributes.Name != "rowspan" && attributes.Name != "style")
                        {
                            listAttributesRemove.Add(attributes.Name);
                        }
                    }
                    //remove att not use
                    if (listAttributesRemove.Count > 0)
                    {
                        foreach (var item in listAttributesRemove)
                        {
                            tableNode.Attributes[item].Remove();
                        }
                    }
                }

            }

            //remove att in td
            var listTDNode = doc.DocumentNode.SelectNodes("//td");
            if (listTDNode != null)
            {
                foreach (var tdNode in listTDNode)
                {
                    //remove att of td
                    var currentTdAttributes = tdNode.Attributes;
                    List<string> listAttributesRemove = new List<string>();
                    //get list att remove
                    foreach (var attributes in currentTdAttributes)
                    {
                        if (attributes.Name != "class" && attributes.Name != "colspan" && attributes.Name != "rowspan" && attributes.Name != "style")
                        {
                            listAttributesRemove.Add(attributes.Name);
                        }
                    }
                    //remove att not use
                    if (listAttributesRemove.Count > 0)
                    {
                        foreach (var item in listAttributesRemove)
                        {
                            tdNode.Attributes[item].Remove();
                        }
                    }
                }
            }
            //remove att in span
            var listSPANNode = doc.DocumentNode.SelectNodes("//span");
            if (listSPANNode != null)
            {
                foreach (var spanNode in listSPANNode)
                {
                    //remove att of td
                    var currentTdAttributes = spanNode.Attributes;
                    List<string> listAttributesRemove = new List<string>();
                    //get list att remove
                    foreach (var attributes in currentTdAttributes)
                    {
                        if (attributes.Name != "class" && attributes.Name != "style")
                        {
                            listAttributesRemove.Add(attributes.Name);
                        }
                    }
                    //remove att not use
                    if (listAttributesRemove.Count > 0)
                    {
                        foreach (var item in listAttributesRemove)
                        {
                            spanNode.Attributes[item].Remove();
                        }
                    }
                }
            }
            #endregion
            #region change div to p
            while ((node = doc.DocumentNode.SelectSingleNode("//div")) != null)
            {
                var p = doc.CreateElement("p");
                p.InnerHtml = node.InnerHtml;
                var currentAttributes = node.Attributes;
                if (currentAttributes.Count > 0)
                {
                    foreach (var attributes in currentAttributes)
                    {
                        p.SetAttributeValue(attributes.Name, attributes.Value);
                    }
                }
                node.ParentNode.InsertAfter(p, node);
                node.Remove();
            }
            #endregion
            #region remove attributes not use
            while ((node = doc.DocumentNode.SelectSingleNode("//@align")) != null)
            {
                node.Attributes["align"].Remove();
            }
            while ((node = doc.DocumentNode.SelectSingleNode("//@type")) != null)
            {
                node.Attributes["type"].Remove();
            }
            while ((node = doc.DocumentNode.SelectSingleNode("//@_fl")) != null)
            {
                node.Attributes["_fl"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@dir")) != null)
            {
                node.Attributes["dir"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@border")) != null)
            {
                node.Attributes["border"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@startcont")) != null)
            {
                node.Attributes["startcont"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@w")) != null)
            {
                node.Attributes["w"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@h")) != null)
            {
                node.Attributes["h"].Remove();
            }

            while ((node = doc.DocumentNode.SelectSingleNode("//@name")) != null)
            {
                node.Attributes["name"].Remove();
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
            #region style element
            while ((node = doc.DocumentNode.SelectSingleNode("//style")) != null)
            {
                node.Remove();
            }
            #endregion
            #region script element
            while ((node = doc.DocumentNode.SelectSingleNode("//script")) != null)
            {
                node.Remove();
            }
            #endregion
            #region embed element
            while ((node = doc.DocumentNode.SelectSingleNode("//embed")) != null)
            {
                node.Remove();
            }
            #endregion
            #region object element
            while ((node = doc.DocumentNode.SelectSingleNode("//object")) != null)
            {
                node.Remove();
            }
            #endregion
            #region style attr
            while ((node = doc.DocumentNode.SelectSingleNode("//@style")) != null)
            {
                string styleContent = node.Attributes["style"].Value;
                string classChange = "";
                //change style text-align: center -> class: text-center
                if (styleContent.Contains("text-align: center") || styleContent.Contains("text-align:center"))
                {
                    classChange += "";
                }
                //change style text-align: right -> class: text-right
                if (styleContent.Contains("text-align: right") || styleContent.Contains("text-align:right"))
                {
                    classChange += "";
                }
                //change style font-style: italic -> class: text-italic
                if (styleContent.Contains("font-style: italic") || styleContent.Contains("font-style:italic"))
                {
                    classChange += "";
                }

                //change style font-weight: bold -> class: text-bold
                if ((styleContent.Contains("font-weight: bold") || styleContent.Contains("font-weight: 600") || styleContent.Contains("font-weight: 700") || styleContent.Contains("font-weight: 800") || styleContent.Contains("font-weight: 900"))
                    || (styleContent.Contains("font-weight:bold") || styleContent.Contains("font-weight:600") || styleContent.Contains("font-weight:700") || styleContent.Contains("font-weight:800") || styleContent.Contains("font-weight:900")))
                {
                    classChange += "";
                }

                //change style bg order -> class: title-order
                if (styleContent.Contains("http://m.quangtrung.vn/Images/Number/"))
                {
                    classChange += "";
                }

                if (!string.IsNullOrEmpty(classChange))
                {
                    if (node.Attributes["class"] != null)
                    {
                        node.Attributes["class"].Value = node.Attributes["class"].Value + " " + classChange;
                    }
                    else
                    {
                        node.SetAttributeValue("class", classChange.Trim());
                    }
                }

                var Attributes = node.Attributes;

                node.Attributes["style"].Remove();
            }
            #endregion

            string outputHtml = doc.DocumentNode.OuterHtml;

            outputHtml = new Regex("/(\n)+/g").Replace(outputHtml, "");

            return outputHtml;
        }
        catch (Exception ex)
        {
            //log.ErrorFormat("Message: {0} - Action: {1} - Error: {2}", ex.Message, "ChangeHTML_CleanStyle", ex);
            return inputHtml;
        }

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
                        var ampImageNode = doc.CreateElement("img");
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
                            if (imageCommentNode.Name != "img")
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
                        ampParentNode.AppendChild(ampImageNode);
                        ampParentNode.AppendChild(ampImageCommentNode);

                    }

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
    public static string ConvertContentToFBInstant(string inputHtml)
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
                        var fbParentNode = doc.CreateElement("figure");
                        var fbImageNode = doc.CreateElement("img");
                        var fbImageCommentNode = doc.CreateElement("figcaption");

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
                                fbImageNode.SetAttributeValue(attributes.Name, attributes.Value);
                            }
                        }

                        //change image comment node
                        if (imageCommentNode != null)
                        {
                            if (imageCommentNode.Name != "img")
                            {
                                fbImageCommentNode.InnerHtml = imageCommentNode.InnerHtml;
                                imageCommentNode.Remove();
                            }
                        }

                        //append element
                        imageNode.ParentNode.InsertAfter(fbParentNode, imageNode);
                        imageNode.Remove();
                        //add layout att to amp image
                        fbImageNode.SetAttributeValue("layout", "responsive");
                        fbParentNode.AppendChild(fbImageNode);
                        fbParentNode.AppendChild(fbImageCommentNode);
                    }

                }
            }
            #endregion

            #region Iframe
            var listIframeNode = doc.DocumentNode.SelectNodes("//iframe");

            if (listIframeNode != null)
            {
                foreach (var iframeNode in listIframeNode)
                {
                    var fbIframeParentNode = doc.CreateElement("figure");
                    fbIframeParentNode.SetAttributeValue("class", "op-interactive");
                    var fbIframeNode = doc.CreateElement("iframe");
                    var listAttr = iframeNode.Attributes;
                    if (listAttr != null)
                    {
                        foreach (var attr in listAttr)
                        {
                            fbIframeNode.SetAttributeValue(attr.Name, attr.Value);
                        }
                    }
                    else
                    {
                        fbIframeNode.SetAttributeValue("allowfullscreen", "");
                        fbIframeNode.SetAttributeValue("frameborder", "0");

                        if (iframeNode.Attributes["width"] != null)
                        {
                            fbIframeNode.SetAttributeValue("width", iframeNode.Attributes["width"].Value);
                        }
                        else
                        {
                            fbIframeNode.SetAttributeValue("width", "0");
                        }
                        if (iframeNode.Attributes["height"] != null)
                        {
                            fbIframeNode.SetAttributeValue("height", iframeNode.Attributes["height"].Value);
                        }
                        else
                        {
                            fbIframeNode.SetAttributeValue("height", "0");
                        }
                    }
                    iframeNode.ParentNode.InsertAfter(fbIframeParentNode, iframeNode);
                    fbIframeParentNode.AppendChild(fbIframeNode);
                    iframeNode.Remove();
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

}


public class FeedRss
{
    private XmlDocument _rss = null;
    public struct RssChannel
    {
        public string Title;
        public string Link;
        public string Description;
    }

    public struct RssItem
    {
        public string Title;
        public string CreateDate;
        public string Link;
        public string guid;
        public string Description;
        public string contentendcodeElement;
    }

    private static XmlDocument addRssChannel(XmlDocument xmlDocument, RssChannel channel)
    {
        XmlElement channelElement = xmlDocument.CreateElement("channel");

        XmlNode rssElement = xmlDocument.SelectSingleNode("rss");

        rssElement.AppendChild(channelElement);

        XmlElement titleElement = xmlDocument.CreateElement("title");

        titleElement.InnerText = channel.Title;

        channelElement.AppendChild(titleElement);

        XmlElement linkElement = xmlDocument.CreateElement("link");

        linkElement.InnerText = channel.Link;

        channelElement.AppendChild(linkElement);

        XmlElement descriptionElement = xmlDocument.CreateElement("description");

        descriptionElement.InnerText = channel.Description;

        channelElement.AppendChild(descriptionElement);

        // Generator information

        XmlElement generatorElement = xmlDocument.CreateElement("generator");

        generatorElement.InnerText = "Your RSS Generator name and version";

        channelElement.AppendChild(generatorElement);

        return xmlDocument;
    }

    private static XmlDocument addRssItem(XmlDocument xmlDocument, RssItem item)
    {
        XmlElement itemElement = xmlDocument.CreateElement("item");

        XmlNode channelElement = xmlDocument.SelectSingleNode("rss/channel");
        XmlElement titleElement = xmlDocument.CreateElement("title");
        titleElement.InnerText = item.Title;
        itemElement.AppendChild(titleElement);

        XmlElement linkElement = xmlDocument.CreateElement("link");
        linkElement.InnerText = item.Link;
        itemElement.AppendChild(linkElement);
        XmlElement guidElement = xmlDocument.CreateElement("guid");
        guidElement.InnerText = item.guid;
        itemElement.AppendChild(guidElement);
        XmlElement dateElement = xmlDocument.CreateElement("pubDate");
        dateElement.InnerText = item.CreateDate;
        itemElement.AppendChild(dateElement);
        XmlElement descriptionElement = xmlDocument.CreateElement("description");
        descriptionElement.InnerText = item.Description;
        itemElement.AppendChild(descriptionElement);

        XmlElement contentendcodeElement = xmlDocument.CreateElement("content", "encoded", "http://tempuri.org/foo");
        contentendcodeElement.InnerText = item.contentendcodeElement;
        itemElement.AppendChild(contentendcodeElement);
        // append the item

        channelElement.AppendChild(itemElement);

        return xmlDocument;
    }

    public FeedRss()
    {
        _rss = new XmlDocument();
        XmlDeclaration xmlDeclaration = _rss.CreateXmlDeclaration("1.0", "utf-8", null);
        _rss.InsertBefore(xmlDeclaration, _rss.DocumentElement);

        XmlElement rssElement = _rss.CreateElement("rss");
        XmlAttribute rssVersionAttribute = _rss.CreateAttribute("version");
        XmlAttribute rssVersionAttribute2 = _rss.CreateAttribute("xmlns:content");

        rssVersionAttribute.InnerText = "2.0";
        rssVersionAttribute2.InnerText = "http://purl.org/rss/1.0/modules/content/";
        rssElement.Attributes.Append(rssVersionAttribute);
        rssElement.Attributes.Append(rssVersionAttribute2);

        _rss.AppendChild(rssElement);

    }

    public void AddRssChannel(RssChannel channel)
    {
        _rss = addRssChannel(_rss, channel);
    }

    public void AddRssItem(RssItem item)
    {
        _rss = addRssItem(_rss, item);
    }

    public string RssDocument
    {
        get
        {
            return _rss.OuterXml;
        }
    }

    public XmlDocument RssXMLDocument
    {
        get
        {
            return _rss;
        }
    }
}