using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;
using NVCMS.Modules.TinTuc;
using System.IO;

namespace DesktopModules.TinTuc.Manager.template
{
    public partial class inc_list : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private string PageAddURL = "/quan-tri/quan-tri-tin-tuc-cap-cao/quan-tri-template?mod=edit";
        NV_NewsTemplateController controller = new NV_NewsTemplateController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var list = controller.GetTemplates(PortalId);
            gridView.DataSource = list;
            gridView.DataBind();
        }
        protected void linkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + PageAddURL);
        }
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int templateID = Convert.ToInt32(gridView.Rows[e.RowIndex].Cells[0].Text);
                NV_NewsTemplateInfo template = controller.GetTemplate(PortalId, templateID);
                if (template != null)
                {
                    string filePath = PortalSettings.HomeDirectoryMapPath + "/NewsTemplates/" + template.FilePath;
                    if(File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    controller.DeleteTemplate(templateID);
                    LoadData();
                }
            }
            catch 
            {
                
            }
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0) //Bo qua dong dau
                {
                    HyperLink link = e.Row.Cells[3].FindControl("linkEdit") as HyperLink;
                    if (link != null)
                    {
                        link.NavigateUrl = PageAddURL + "&id=" + e.Row.Cells[0].Text;
                    }
                    e.Row.Cells[2].Text = PortalSettings.HomeDirectory + "NewsTemplates/" + e.Row.Cells[2].Text;

                }
            }
            catch { } 
        }
    }
}