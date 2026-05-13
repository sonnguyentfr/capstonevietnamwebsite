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
using System.Web.Mvc;
using System.IO;
using DotNetNuke.Entities.Portals;

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
    NV_NewsController ctl = new NV_NewsController();
    NewsByMediaController _NewsByMediaController = new NewsByMediaController();
    ArrayList arr = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            XmlDocument xmlDoc = new XmlDocument();
            using (XmlWriter writer = xmlDoc.CreateNavigator().AppendChild())
            {
                //writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                arr = ctl.ShowBaiMoiNhat("", 0, 600);
                foreach (NV_NewsInfo obj in arr)
                {
                    writer.WriteStartElement("url");
                    // required
                    writer.WriteElementString("loc", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title));
                    //Lay het anh theo bai viet
                    ArrayList arrMedia = new ArrayList();
                    arrMedia = _NewsByMediaController._GetAllByNewId(obj.NewId);
                    foreach (NewsByMediaInfo objnewsByMediaInfo in arrMedia)
                    {
                        writer.WriteStartElement("image", "image", null);
                        // start:optional
                        writer.WriteElementString("image", "loc", null, objnewsByMediaInfo.ImageFull.Replace("/DATA", "https://f.thuongtruong.com.vn"));
                        writer.WriteElementString("image", "caption", null, obj.Title);
                        writer.WriteElementString("image", "license", null, "https://thuongtruong.com.vn");
                        writer.WriteElementString("image", "family_friendly", null, "yes");
                        writer.WriteEndElement();
                    }
                    //end::optional
					writer.WriteElementString("lastmod", obj.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.7");
                    writer.WriteEndElement(); //url
                }

                writer.WriteEndElement(); //urlset 
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

}

