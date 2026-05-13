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
    string UrlAlias = "";
    string strImg;
    string itemid = "";
    int month = DateTime.Now.Month;
    int year = DateTime.Now.Year;
    int day = DateTime.Now.Day;
    NewsByTagsController ctl = new NewsByTagsController();
    ArrayList arr = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FeedRss rss = new FeedRss();
            FeedRss.RssChannel channel = new FeedRss.RssChannel();
            channel.Title = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalName;
            channel.Link = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalAlias.HTTPAlias.ToString();
            channel.Description = "RSS Results	only for asset type(s) of article";

            string sUrl1 = Request.RawUrl;
            itemid = Ultis.GetRequestIdXML(sUrl1);
            string TagId = itemid.Replace("-", "+");
            if (itemid != "")
            {
                arr = ctl.NewsByTags_GetByTags_Index(TagId, 1, 500);
                foreach (NV_NewsInfo obj in arr)
                {
                    FeedRss.RssItem item = new FeedRss.RssItem();
                    item.loc = Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title); ;
                    item.lastmod = obj.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz");
                    item.changefreq = "hourly";
                    item.priority = "0.5";
                    rss.AddRssItem(item);
                }
            }


            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write(rss.RssDocument);
            Response.End();
            //Response.Write(TagId);
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
    }

    private static XmlDocument addRssChannel(XmlDocument xmlDocument, RssChannel channel)
    {
        XmlElement channelElement = xmlDocument.CreateElement("channel");
        XmlNode rssElement = xmlDocument.SelectSingleNode("urlset");
        rssElement.AppendChild(channelElement);
        return xmlDocument;
    }

    private static XmlDocument addRssItem(XmlDocument xmlDocument, RssItem item)
    {
        XmlElement itemElement = xmlDocument.CreateElement("url");
        XmlNode channelElement = xmlDocument.SelectSingleNode("urlset");
        XmlElement locElement = xmlDocument.CreateElement("loc");
        locElement.InnerText = item.loc;
        itemElement.AppendChild(locElement);
        XmlElement lastmodElement = xmlDocument.CreateElement("lastmod");
        lastmodElement.InnerText = item.lastmod;
        itemElement.AppendChild(lastmodElement);
        XmlElement changefreqElement = xmlDocument.CreateElement("changefreq");
        changefreqElement.InnerText = item.changefreq;
        itemElement.AppendChild(changefreqElement);
        //--
        XmlElement priorityElement = xmlDocument.CreateElement("priority");
        priorityElement.InnerText = item.priority;
        itemElement.AppendChild(priorityElement);
        // append the item

        channelElement.AppendChild(itemElement);

        return xmlDocument;
    }

    public FeedRss()
    {
        _rss = new XmlDocument();
        XmlDeclaration xmlDeclaration = _rss.CreateXmlDeclaration("1.0", "utf-8", null);
        _rss.InsertBefore(xmlDeclaration, _rss.DocumentElement);

        XmlElement rssElement = _rss.CreateElement("urlset");
        XmlAttribute rssVersionAttribute = _rss.CreateAttribute("xmlns");
        rssVersionAttribute.InnerText = "http://www.sitemaps.org/schemas/sitemap/0.9";
        rssElement.Attributes.Append(rssVersionAttribute);
        XmlAttribute rssVersionAttribute1 = _rss.CreateAttribute("xmlns:xsi");
        rssVersionAttribute1.InnerText = "http://www.w3.org/2001/XMLSchema-instance";
        rssElement.Attributes.Append(rssVersionAttribute1);

        XmlAttribute rssVersionAttribute1a = _rss.CreateAttribute("xsi:schemaLocation");
        rssVersionAttribute1a.InnerText = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
        rssElement.Attributes.Append(rssVersionAttribute1a);
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