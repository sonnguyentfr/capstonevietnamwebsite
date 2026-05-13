using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;

namespace DesktopModules.TinTuc.Manager.template
{
    public partial class Main : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string control = DotNetNuke.Common.Globals.ResolveUrl(this.TemplateSourceDirectory +"/inc_list.ascx");
            DotNetNuke.Entities.Modules.PortalModuleBase module;
            if (Request.QueryString["mod"] != null)
            {                
                switch (Request.QueryString["mod"])
                {
                    case "list":
                        control = DotNetNuke.Common.Globals.ResolveUrl(this.TemplateSourceDirectory + "/inc_list.ascx");
                        break;
                    case "edit":
                        control = DotNetNuke.Common.Globals.ResolveUrl(this.TemplateSourceDirectory + "/inc_edit.ascx");
                        break;
                    default:
                        break;
                        
                }                
            }
            try
            {
                module = this.LoadControl(control) as DotNetNuke.Entities.Modules.PortalModuleBase;
                module.ModuleConfiguration = this.ModuleConfiguration;
                paneControl.Controls.Add(module);                
            }
            catch { }
        }
    }
}