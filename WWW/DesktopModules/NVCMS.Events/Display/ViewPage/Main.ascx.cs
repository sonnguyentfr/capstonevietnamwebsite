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
using NVCMS.Modules.TinTuc;
namespace DesktopModules.TinTuc.ViewPage
{
    public partial class Main : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PortalModuleBase control = new PortalModuleBase();
            string fbclid = Request.QueryString["fbclid"];
            string sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "");
            string sId = Ultis.GetRequestId2(sUrl);
            string srequest = "";
            if (sId !="")
            {
                srequest = Ultis.GetRequestName(sId);
                if (srequest !="")
                {
                    if (srequest == "d")
                    {
                        control = Page.LoadControl("/DesktopModules/NVCMS.Events/Display/ViewPage/Detail.ascx") as PortalModuleBase;
                    }
                    else
                    {
                        control = Page.LoadControl("/DesktopModules/NVCMS.Events/Display/ViewPage/Index.ascx") as PortalModuleBase;
                    }
                }
                else
                {
                    control = Page.LoadControl("/DesktopModules/NVCMS.Events/Display/ViewPage/Index.ascx") as PortalModuleBase;
                }
            }
            else
            {
                control = Page.LoadControl("/DesktopModules/NVCMS.Events/Display/ViewPage/Index.ascx") as PortalModuleBase;
                
            }
            control.ModuleConfiguration = ModuleConfiguration;
            plhNews.Controls.Add(control);
            
        }
    }
}