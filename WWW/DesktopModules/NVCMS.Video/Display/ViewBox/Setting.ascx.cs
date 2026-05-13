using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;
using DotNetNuke.Entities.Modules;
using NVCMS.Modules.Video;
using DotNetNuke.Common.Utilities;
using System.Collections;
using DotNetNuke.Entities.Tabs;

namespace DesktopModules.TinTuc.View
{
    public partial class Setting : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private Video_TemplateController templateController = new Video_TemplateController();
        private ModuleController moduleController = new ModuleController();
        public override void LoadSettings()
        {
            try
            {
                BindTemplate();
                BindTab();
                if (!Null.IsNull(ModuleSettings[BL.settingView_ImgSize.ToString()]))
                {

                    string[] sImageSize = ModuleSettings[BL.settingView_ImgSize.ToString()].ToString().Split(';');
                    string[] sSizeTop = sImageSize[0].Split(',');
                    string[] sSizeMore = sImageSize[1].Split(',');

                    txtTopWidth.Text = sSizeTop[0];
                    txtTopHeight.Text = sSizeTop[1];
                    txtMoreWidth.Text = sSizeMore[0];
                    txtMoreHeight.Text = sSizeMore[1];
                }

                if (!Null.IsNull(ModuleSettings[BL.settingView_Total.ToString()]))
                {
                    string[] sTotal = ModuleSettings[BL.settingView_Total.ToString()].ToString().Split(';');
                    txtNewsTop.Text = sTotal[0];
                    txtNewsMore.Text = sTotal[1];
                }
                
            }
            catch { }
        }

        public override void UpdateSettings()
        {
            string totalTop = txtNewsTop.Text != "" ? txtNewsTop.Text : "0";
            string totalMore = txtNewsMore.Text != "" ? txtNewsMore.Text : "0";
            string imgTopWidth = txtTopWidth.Text != "" ? txtTopWidth.Text : "0";
            string imgTopHeight = txtTopHeight.Text != "" ? txtTopHeight.Text : "0";
            string imgMoreWidth = txtMoreWidth.Text != "" ? txtMoreWidth.Text : "0";
            string imgMoreHeight = txtMoreHeight.Text != "" ? txtMoreHeight.Text : "0";

            string total = totalTop + ";" + totalMore;
            string imgSize = imgTopWidth + "," + imgTopHeight + ";" + imgMoreWidth + "," + imgMoreHeight;


            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Total.ToString(), total);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Template.ToString(), dropTemplate.SelectedValue);
            moduleController.UpdateModuleSetting(ModuleId, "VideoSettingPage", drlTabID.SelectedValue);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_ImgSize.ToString(), imgSize);
            DataCache.ClearCache();
        }

        
        private void BindTemplate()
        {
            var list = templateController.GetTemplates(0);
            dropTemplate.DataSource = list;
            dropTemplate.DataTextField = "TemplateName";
            dropTemplate.DataValueField = "FilePath";
            dropTemplate.DataBind();
            if (!Null.IsNull(ModuleSettings[BL.settingView_Template.ToString()]))
            {
                dropTemplate.SelectedValue = ModuleSettings[BL.settingView_Template].ToString();
            }
        }
        private void BindTab()
        {
            var list = TabController.GetPortalTabs(PortalId, -1, true, false);
            drlTabID.DataSource = list;
            drlTabID.DataTextField = "IndentedTabName";
            drlTabID.DataValueField = "tabid";
            drlTabID.DataBind();
            if (!Null.IsNull(ModuleSettings["VideoSettingPage"]))
            {
                drlTabID.SelectedValue = ModuleSettings["VideoSettingPage"].ToString();
            }
        }
    }
}