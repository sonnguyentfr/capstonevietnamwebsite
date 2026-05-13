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

                // add namespaces
                writer.WriteAttributeString("xmlns", "video", null, "http://www.google.com/schemas/sitemap-video/1.1");
                arr = ctl.ShowBaiVideo(0, 600);
                foreach (NV_NewsVideoInfo obj in arr)
                {
                    writer.WriteStartElement("url");

                    // required
                    writer.WriteElementString("loc", Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title));
                    writer.WriteStartElement("video", "video", null);

                    // start:optional
                    writer.WriteElementString("video", "thumbnail_loc", null, obj.ImagePath.Replace("/DATA", "https://f.thuongtruong.com.vn"));
                    writer.WriteElementString("video", "title", null, obj.Title);
                    writer.WriteElementString("video", "description", null, obj.Summary);
                    writer.WriteElementString("video", "content_loc", null, obj.MediaUrl.Replace("/DATA", "https://f.thuongtruong.com.vn"));

                    writer.WriteStartElement("video", "player_loc", null);
                    writer.WriteAttributeString("autoplay", "ap=1");
                    writer.WriteString(obj.MediaUrl.Replace("/DATA", "https://f.thuongtruong.com.vn"));
                    writer.WriteEndElement(); // video:player_loc
                                              // end:optional

                    writer.WriteElementString("video", "duration", null, Ultis.GetVideoDurationSecond(obj.forder + "/" + obj.filename, obj.NewId));
                    writer.WriteElementString("video", "publication_date", null, obj.PublishedDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    writer.WriteElementString("video", "category", null, obj.CategoryName);

                    //writer.WriteStartElement("video", "restriction", null);
                    //writer.WriteAttributeString("relationship", "allow");
                    //writer.WriteString("IE GB US CA");
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("video", "gallery_loc", null);
                    //writer.WriteAttributeString("title", "Cooking Videos");
                    //writer.WriteString("http://cooking.example.com");
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("video", "price", null);
                    //writer.WriteAttributeString("currency", "EUR");
                    //writer.WriteString("1.99");
                    //writer.WriteEndElement();

                    //writer.WriteElementString("video", "requires_subscription", null, "yes");

                    //writer.WriteStartElement("video", "uploader", null);
                    //writer.WriteAttributeString("info", "http://www.example.com/users/grillymcgrillerson");
                    //writer.WriteString("GrillyMcGrillerson");
                    //writer.WriteEndElement();

                    writer.WriteElementString("video", "live", null, "No");

                    writer.WriteEndElement(); // video:video
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

