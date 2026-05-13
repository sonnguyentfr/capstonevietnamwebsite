using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using DotNetNuke;
using DotNetNuke.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common.Utilities;
using NVCMS.Modules.Video;
namespace DesktopModules.Video.ViewPage
{
    public partial class Main : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fbclid = Request.QueryString["fbclid"];
            string sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "");
            int sId = Ultis.GetRequestId(sUrl);
            PortalModuleBase control = new PortalModuleBase();
            if (sId > 0) //Index
            {
                control = Page.LoadControl("~/DesktopModules/NVCMS.Video/Display/ViewPage/Detail.ascx") as PortalModuleBase;
               
            }
            else  //Details
            {
                control = Page.LoadControl("/DesktopModules/NVCMS.Video/Display/ViewPage/Index.ascx") as PortalModuleBase;
            }
            //Response.Write(sId);
            control.ModuleConfiguration = ModuleConfiguration;
            plhNews.Controls.Add(control);
        }
    }
}