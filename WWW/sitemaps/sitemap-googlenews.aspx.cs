using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Web.Configuration;
using NVCMS.Modules.TinTuc;
using System.Collections;
using System.IO;

public partial class feedsgoogle : System.Web.UI.Page
{
    string strLit;
    string strUrl;
    string UrlAlias = "";
    string strImg;

    NV_NewsController ctl = new NV_NewsController();
    ArrayList arr = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {

        XmlDocument xmlDoc = new XmlDocument();
        //using (XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8))
        using (XmlWriter writer = xmlDoc.CreateNavigator().AppendChild())
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            writer.WriteAttributeString("xmlns", "news", null, "http://www.google.com/schemas/sitemap-news/0.9");
            arr = ctl.ShowBaiMoiNhat("", 0, 1000);
            foreach (NV_NewsInfo obj in arr)
            {
                writer.WriteStartElement("url");

                writer.WriteElementString("loc", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title));
                writer.WriteStartElement("news", "news", null);
                writer.WriteStartElement("news", "publication", null);
                writer.WriteElementString("news", "name", null, obj.Title);
                writer.WriteElementString("news", "language", null, "vi");
                writer.WriteEndElement();


                writer.WriteElementString("news", "genres", null, obj.CategoryName);
                writer.WriteElementString("news", "publication_date", null, obj.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                writer.WriteElementString("news","title",null, obj.Title);
                writer.WriteElementString("news","keywords",null, obj.Tags);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        var stringWriter = new StringWriter();
        var xmlTextWriter = XmlWriter.Create(stringWriter);
        xmlDoc.WriteTo(xmlTextWriter);
        xmlTextWriter.Flush();
        Response.Clear();
        Response.ContentType = "text/xml";
        Response.Charset = "utf-8";
        var ketquar = stringWriter.GetStringBuilder().ToString().Replace("utf-16", "utf-8");
        Response.Write(ketquar);
        Response.End();





    }
}
