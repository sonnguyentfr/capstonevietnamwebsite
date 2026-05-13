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
            if (!String.IsNullOrEmpty(Request.QueryString["itemid"]))
            {
                // Query string value is there so now use it
                itemid = Convert.ToInt32(Request.QueryString["itemid"]);
            }
            NV_NewsInfo objNews = ctl.GetByID(itemid);
            if (objNews != null)
            {
                FeedRss rss = new FeedRss();
                FeedRss.RssChannel channel = new FeedRss.RssChannel();
                channel.Title = objNews.Title.ToString();
                channel.Link = Ultis.FormatLink(Convert.ToInt32(BL.GetMappingCategoryIDByTabID(objNews.CategoryId)), objNews.NewId, objNews.Title);
                channel.Description = objNews.Summary.ToString();
                channel.lastBuildDate = objNews.PublishedDate.ToString("r");
                rss.AddRssChannel(channel);
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write(rss.RssDocument);
                Response.End();
            }

            
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
        public string lastBuildDate;
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
        XmlElement lastBuildDateElement = xmlDocument.CreateElement("lastBuildDate");
        lastBuildDateElement.InnerText = channel.lastBuildDate;
        channelElement.AppendChild(lastBuildDateElement);
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
        XmlAttribute rssVersionAttribute3 = _rss.CreateAttribute("xmlns:dc");
        XmlAttribute rssVersionAttribute4 = _rss.CreateAttribute("xmlns:atom");
        XmlAttribute rssVersionAttribute5 = _rss.CreateAttribute("xmlns:sy");

        rssVersionAttribute.InnerText = "2.0";
        rssVersionAttribute2.InnerText = "http://purl.org/rss/1.0/modules/content/";
        rssVersionAttribute3.InnerText = "http://purl.org/dc/elements/1.1/";
        rssVersionAttribute4.InnerText = "http://www.w3.org/2005/Atom";
        rssVersionAttribute5.InnerText = "http://purl.org/rss/1.0/modules/syndication/";
        rssElement.Attributes.Append(rssVersionAttribute);
        rssElement.Attributes.Append(rssVersionAttribute2);
        rssElement.Attributes.Append(rssVersionAttribute3);
        rssElement.Attributes.Append(rssVersionAttribute4);
        rssElement.Attributes.Append(rssVersionAttribute5);
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