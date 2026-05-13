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
    string itemid = "";
    string strUrl;
    string UrlAlias ="";
    int month = DateTime.Now.Month;
    int year = DateTime.Now.Year;
    DateTime lastDay = DateTime.Now.Date;
    NV_NewsController ctl = new NV_NewsController();
    string strImg;
    ArrayList arr = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FeedRss rss = new FeedRss();
            FeedRss.RssChannel channel = new FeedRss.RssChannel();
            channel.Title = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalName;
            channel.Link = DotNetNuke.Entities.Portals.PortalSettings.Current.PortalAlias.HTTPAlias.ToString();
            channel.Description ="RSS Results	only for asset type(s) of article";
            //Get param
            string sUrl1 = Request.RawUrl;
            itemid = Ultis.GetRequestIdXML(sUrl1);
            //add cat vao sitemap
            FeedRss.RssItem items = new FeedRss.RssItem();
            if (itemid != "")
            {
                string[] sTags = itemid.Split('-');
                for (int i = 0; i <= sTags.Length - 1; i++)
                {
                    year = Convert.ToInt16(sTags[0]);
                    month = Convert.ToInt16(sTags[1]);
                }
            }
            if (year == (DateTime.Now.Year) & month == (DateTime.Now.Month)) {
                lastDay = DateTime.Now.Date;
            }
            else
            {
                lastDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

            }
            for (int i = lastDay.Day; i >= 1; i--)
            {
                FeedRss.RssItem item = new FeedRss.RssItem();
                item.loc = "https://thuongtruong.com.vn/sitemaps/newslist/" + year + "-" + month + "-" + i + ".xml";
                item.lastmod = lastDay.ToString("yyyy-MM-dd");
                rss.AddRssItem(item);
            }
            Response.Clear();
            Response.ContentType = "text/xml";
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
    }

    private static XmlDocument addRssChannel(XmlDocument xmlDocument, RssChannel channel)
    {
        XmlElement channelElement = xmlDocument.CreateElement("channel");

        XmlNode rssElement = xmlDocument.SelectSingleNode("sitemapindex");

        rssElement.AppendChild(channelElement);

        return xmlDocument;
    }

    private static XmlDocument addRssItem(XmlDocument xmlDocument, RssItem item)
    {
        XmlElement itemElement = xmlDocument.CreateElement("sitemap");

        XmlNode channelElement = xmlDocument.SelectSingleNode("sitemapindex");

        XmlElement locElement = xmlDocument.CreateElement("loc");
        locElement.InnerText = item.loc;
        itemElement.AppendChild(locElement);
        XmlElement lastmodElement = xmlDocument.CreateElement("lastmod");
        lastmodElement.InnerText = item.lastmod;
        itemElement.AppendChild(lastmodElement);
        // append the item
        channelElement.AppendChild(itemElement);
        return xmlDocument;
    }

    public FeedRss()
    {
        _rss = new XmlDocument();
        XmlDeclaration xmlDeclaration = _rss.CreateXmlDeclaration("1.0","utf-8", null);
        _rss.InsertBefore(xmlDeclaration, _rss.DocumentElement);

        XmlElement rssElement = _rss.CreateElement("sitemapindex");
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