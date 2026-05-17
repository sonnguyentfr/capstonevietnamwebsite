using NVCMS.Modules.EventsWebsite;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace DesktopModules.NV_Events.Manager.template
{
    public partial class inc_list : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        Events_TemplateController controller = new Events_TemplateController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var list = controller._GetAll(PortalId);
            gridView.DataSource = list;
            gridView.DataBind();
        }
        protected void linkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL() + "?mod=edit");
        }
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int templateID = Convert.ToInt32(gridView.Rows[e.RowIndex].Cells[0].Text);
                Events_TemplateInfo template = controller._GetByID(templateID, PortalId);

                if (template != null)
                {
                    string filePath = "/Portals/0/EventsTemplates/" + template.FilePath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    controller._Delete(templateID, 0);
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
                        link.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL() + "?mod=edit&id=" + e.Row.Cells[0].Text;
                    }
                    e.Row.Cells[2].Text = "/Portals/0/EventsTemplates/" + e.Row.Cells[2].Text;

                }
            }
            catch { }
        }
    }
}