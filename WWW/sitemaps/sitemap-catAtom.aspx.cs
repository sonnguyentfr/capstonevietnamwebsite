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
using DotNetNuke.Entities.Portals;

public partial class feeds : System.Web.UI.Page
{
    string strLit;
    string strUrl;
    string UrlAlias = "";
    string strImg;
    string itemid;
    NV_NewsController ctl = new NV_NewsController();
    NV_NewsCategoriesController _NV_NewsCategoriesController = new NV_NewsCategoriesController();
    public string CatLink { get; set; }
    public string CatName { get; set; }
    public string CatSummary { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUrl1 = Request.RawUrl;
            itemid = Ultis.GetRequestIdXML(sUrl1);
            int itemr = 0;
            //Bat dau cat chuoi
            int iLength = 0;
            int iStart = itemid.LastIndexOf("-", System.StringComparison.Ordinal);
            int iEnd = itemid.Length;
            if (iEnd > 0)
            {
                iLength = iEnd - iStart - 1;
                itemr =Convert.ToInt32( itemid.Substring(iStart + 1, iLength));
            }
            NV_NewsCategoriesInfo objNV_NewsCategoriesInfo = _NV_NewsCategoriesController.GetByID(itemr);
            CatLink = DotNetNuke.Common.Globals.NavigateURL(objNV_NewsCategoriesInfo.TabID);
            CatName = RemoveIllegalCharacters(objNV_NewsCategoriesInfo.CategoryName);
            CatSummary = RemoveIllegalCharacters(objNV_NewsCategoriesInfo.Description);
            this.Response.ContentType = "text/xml";
            RepeaterRSS.DataSource = ctl.ShowBaiMoiDanhMuc("", itemr, 0, 1000, false);
            RepeaterRSS.DataBind();

            //Response.Write(DotNetNuke.Common.Globals.NavigateURL(BL.GetMappingTabIDByCategoryID(1)));
        }
    }
    protected string RemoveIllegalCharacters(object input)
    {
        // cast the input to a string
        string data = input.ToString();
        // replace illegal characters in XML documents with their entity references
        data = data.Replace("&", "&amp;");
        data = data.Replace("\"", "&quot;");
        data = data.Replace("'", "&apos;");
        data = data.Replace("<", "&lt;");
        data = data.Replace(">", "&gt;");

        return data;
    }
}
