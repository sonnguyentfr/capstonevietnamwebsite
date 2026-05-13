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
using System.IO;

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
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.ContentType = "text/xml";
        RssWriter writer = new RssWriter(Response.OutputStream);
        writer.WriteStartElement(RssElements.Rss);
        writer.WriteAttributeString(RssAttributes.Version, "2.0");
        writer.WriteStartElement(RssElements.Channel);
        writer.WriteElementString(RssElements.Title, "Tạp chí Thương Trường");
        writer.WriteElementString(RssElements.Link, "https://thuongtruong.com.vn");
        writer.WriteElementString(RssElements.Description, "Tạp chí Thương Trường");
        writer.WriteElementString(RssElements.Copyright, "Copyright (C) Tạp chí Thương Trường");
        writer.WriteElementString(RssElements.Generator, "thuongtruong.com.vn XML RSS Generator");
        NV_NewsController ctl = new NV_NewsController();
        arr = ctl.SelectIndex("", 0, 0, 1, 1000, "", false);

        foreach (NV_NewsInfo obj in arr)
        {
            string id = obj.NewId.ToString();
            string subject = obj.Title.ToString();
            string description = obj.Summary.ToString();
            DateTime dt = DateTime.Parse(obj.PublishedDate.ToString());
            string date = dt.ToString("yyyy-MM-ddTHH:mm:sszzz");
            writer.WriteStartElement(RssElements.Item);
            writer.WriteElementString(RssElements.Title, subject);
            writer.WriteElementString(RssElements.Link, Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title));

            writer.WriteElementString(RssElements.Description, description);
            writer.WriteElementString(RssElements.PubDate, date);
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.Close();
        Response.End();
    }
}
public enum RssElements
{
    Rss, Channel, Title, Description, Link, Copyright, Generator, Item, PubDate
}
public enum RssAttributes
{
    Version
}

/// <summary>
/// RSS writer that emits RSS feeds. 
/// </summary>
public class RssWriter : XmlWriter
{
    private XmlWriter writer;
    private Stream objStream;

    #region Constructor
    public RssWriter(Stream stream)
    {
        objStream = stream;
        writer = XmlWriter.Create(objStream);
    }
    #endregion

    #region Stream Related Operations

    public override void Close()
    {
        objStream.Close();
        writer.Close();
    }

    public override void Flush()
    {
        writer.Flush();
    }

    #endregion

    #region Writing Elements
    public void WriteStartElement(RssElements element)
    {
        string elementName = "";

        switch (element)
        {
            case RssElements.Channel:
                elementName = "channel";
                break;
            case RssElements.Copyright:
                elementName = "copyright";
                break;
            case RssElements.Description:
                elementName = "description";
                break;
            case RssElements.Generator:
                elementName = "generator";
                break;
            case RssElements.Item:
                elementName = "item";
                break;
            case RssElements.Link:
                elementName = "link";
                break;
            case RssElements.PubDate:
                elementName = "pubDate";
                break;
            case RssElements.Rss:
                elementName = "rss";
                break;
            case RssElements.Title:
                elementName = "title";
                break;
        }
        writer.WriteStartElement(elementName);
    }
    public void WriteElementString(RssElements element, string value)
    {
        string elementName = "";

        switch (element)
        {
            case RssElements.Channel:
                elementName = "channel";
                break;
            case RssElements.Copyright:
                elementName = "copyright";
                break;
            case RssElements.Description:
                elementName = "description";
                break;
            case RssElements.Generator:
                elementName = "generator";
                break;
            case RssElements.Item:
                elementName = "item";
                break;
            case RssElements.Link:
                elementName = "link";
                break;
            case RssElements.PubDate:
                elementName = "pubDate";
                break;
            case RssElements.Rss:
                elementName = "rss";
                break;
            case RssElements.Title:
                elementName = "title";
                break;
        }
        writer.WriteElementString(elementName, value);
    }
    public override void WriteEndElement() { writer.WriteEndElement(); }
    #endregion

    #region Writing Attributes
    public void WriteStartAttribute(RssAttributes attb)
    {
        if (attb == RssAttributes.Version)
        {
            writer.WriteStartAttribute("version");
        }
    }
    public void WriteAttributeString(RssAttributes attb, string value)
    {
        if (attb == RssAttributes.Version)
        {
            writer.WriteAttributeString("version", value);
        }
    }
    public override void WriteEndAttribute() { writer.WriteEndAttribute(); }
    #endregion

    #region Writing Data
    public override void WriteCData(string text) { writer.WriteCData(text); }
    public override void WriteChars(char[] buffer, int index, int count) { writer.WriteChars(buffer, index, count); }
    public override void WriteComment(string text) { writer.WriteComment(text); }
    public override void WriteWhitespace(string ws) { writer.WriteWhitespace(ws); }
    public override void WriteString(string text) { writer.WriteString(text); }
    #endregion

    #region Document
    public override void WriteStartDocument() { writer.WriteStartDocument(); }
    public override void WriteStartDocument(bool standalone) { writer.WriteStartDocument(standalone); }
    public override void WriteEndDocument() { writer.WriteEndDocument(); }
    #endregion

    #region Not Implemented Methods
    void ThrowException() { throw new Exception("The method or operation is not implemented."); }
    public override string LookupPrefix(string ns) { ThrowException(); return ""; }
    public override void WriteBase64(byte[] buffer, int index, int count) { ThrowException(); }
    public override void WriteCharEntity(char ch) { ThrowException(); }
    public override void WriteStartElement(string prefix, string localName, string ns) { ThrowException(); }
    public override WriteState WriteState
    {
        get { throw new Exception("The method or operation is not implemented."); }
    }
    public override void WriteSurrogateCharEntity(char lowChar, char highChar) { ThrowException(); }
    public override void WriteEntityRef(string name) { ThrowException(); }
    public override void WriteFullEndElement() { ThrowException(); }
    public override void WriteProcessingInstruction(string name, string text) { ThrowException(); }
    public override void WriteRaw(string data) { ThrowException(); }
    public override void WriteRaw(char[] buffer, int index, int count) { ThrowException(); }
    public override void WriteStartAttribute(string prefix, string localName, string ns) { ThrowException(); }
    public override void WriteDocType(string name, string pubid, string sysid, string subset) { ThrowException(); }
    #endregion
}