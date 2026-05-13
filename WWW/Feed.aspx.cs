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

public partial class feeds : System.Web.UI.Page
{
    string strLit;
    string strUrl;
    string UrlAlias ="";
    string strImg;
    NV_NewsController ctl = new NV_NewsController();
    ArrayList arr = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FeedRss rss = new FeedRss();
            arr = ctl.SelectIndex("", 0, 0, 1, 500,"", false);
            foreach (NV_NewsInfo obj in arr)
            {
                FeedRss.RssItem item = new FeedRss.RssItem();
                item.Title = obj.Title;
                item.loc = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title);
                item.descriptionElement = obj.Summary ;
                item.changefreq = "daily";
                item.priority = "0.5";
                rss.AddRssItem(item);
            }

            Response.Clear();
            Response.ContentType ="text/xml";
            Response.Write(rss.RssDocument);
            Response.End();
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
        public string changefreq;
        public string loc;
        public string lastmod;
        public string priority;
        public string pubDateElement;
        public string descriptionElement;
    }

    private static XmlDocument addRssItem(XmlDocument xmlDocument, RssItem item)
    {

        XmlNode channelElement = xmlDocument.SelectSingleNode("rss");
        XmlElement itemElement = xmlDocument.CreateElement("item");
        XmlElement titleElement = xmlDocument.CreateElement("title");
        titleElement.InnerText = item.Title;
        itemElement.AppendChild(titleElement);
        XmlElement locElement = xmlDocument.CreateElement("link");
        locElement.InnerText = item.loc;
        itemElement.AppendChild(locElement);
        //XmlElement lastmodElement = xmlDocument.CreateElement("lastmod");
        //lastmodElement.InnerText = item.lastmod;
        //itemElement.AppendChild(lastmodElement);
        XmlElement changefreqElement = xmlDocument.CreateElement("changefreq");
        changefreqElement.InnerText = item.changefreq;
        itemElement.AppendChild(changefreqElement);
        //--
        XmlElement priorityElement = xmlDocument.CreateElement("priority");
        priorityElement.InnerText = item.priority;
        itemElement.AppendChild(priorityElement);

        XmlElement pubDateElement = xmlDocument.CreateElement("pubDate");
        pubDateElement.InnerText = item.pubDateElement;
        itemElement.AppendChild(pubDateElement);

        XmlElement descriptionElement = xmlDocument.CreateElement("description");
        descriptionElement.InnerText = item.descriptionElement;
        itemElement.AppendChild(descriptionElement);
        // append the item

        channelElement.AppendChild(itemElement);

        return xmlDocument;
    }

    public FeedRss()
    {
        _rss = new XmlDocument();
        XmlDeclaration xmlDeclaration = _rss.CreateXmlDeclaration("1.0","utf-8", null);
        _rss.InsertBefore(xmlDeclaration, _rss.DocumentElement);

        XmlElement rssElement = _rss.CreateElement("rss");
        XmlAttribute rssVersionAttribute = _rss.CreateAttribute("xmlns:content");
        rssVersionAttribute.InnerText = "http://purl.org/rss/1.0/modules/content/";
        rssElement.Attributes.Append(rssVersionAttribute);
        XmlAttribute rssVersionAttribute1 = _rss.CreateAttribute("xmlns:wfw");
        rssVersionAttribute1.InnerText = "http://wellformedweb.org/CommentAPI/";
        rssElement.Attributes.Append(rssVersionAttribute1);

        XmlAttribute rssVersionAttribute1a = _rss.CreateAttribute("xmlns:dc");
        rssVersionAttribute1a.InnerText = "http://purl.org/dc/elements/1.1/";
        rssElement.Attributes.Append(rssVersionAttribute1a);
        XmlAttribute rssVersionAttribute2 = _rss.CreateAttribute("xmlns:atom");
        rssVersionAttribute2.InnerText = "http://www.w3.org/2005/Atom";
        rssElement.Attributes.Append(rssVersionAttribute2);

        XmlAttribute rssVersionAttribute3 = _rss.CreateAttribute("xmlns:media");
        rssVersionAttribute3.InnerText = "http://search.yahoo.com/mrss/";
        rssElement.Attributes.Append(rssVersionAttribute3);

        XmlAttribute rssVersionAttribute4 = _rss.CreateAttribute("version");
        rssVersionAttribute4.InnerText = "2.0";
        rssElement.Attributes.Append(rssVersionAttribute4);
        _rss.AppendChild(rssElement);
        
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