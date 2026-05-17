using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;
using DotNetNuke.Entities.Modules;
using NVCMS.Modules.TinTuc;
using DotNetNuke.Common.Utilities;
using System.Collections;

namespace NVCMS.Modules.School
{
    public partial class Setting : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private MarketingSchoolTemplateController templateController = new MarketingSchoolTemplateController();
        private ModuleController moduleController = new ModuleController();
        public override void LoadSettings()
        {
            try
            {
                BindTemplate();
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
                string viewType = "TruongDoiTac,TruongNoBat";
                if (!Null.IsNull(ModuleSettings[BL.settingView_Type.ToString()]))
                {
                    viewType = ModuleSettings[BL.settingView_Type.ToString()].ToString();
                    string[] sTyle = viewType.Split(',');
                    foreach (var item in sTyle)
                    {
                        for (int i = 0; i < checkListNewsConfig.Items.Count; i++)
                        {
                            if (checkListNewsConfig.Items[i].Value == item)
                            {
                                checkListNewsConfig.Items[i].Selected = true;
                            }
                        }

                    }
                }
                if (!Null.IsNull(ModuleSettings[BL.settingView_SizeDes.ToString()]))
                {
                    txtSizeDes.Text = ModuleSettings[BL.settingView_SizeDes.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingView_SizeTitle.ToString()]))
                {
                    txtSizeTitle.Text = ModuleSettings[BL.settingView_SizeTitle.ToString()].ToString();
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
            string sizeDes = txtSizeDes.Text != "" ? txtSizeDes.Text : "0";
            string sizeTitle = txtSizeTitle.Text != "" ? txtSizeTitle.Text : "0";
            string viewType = "";
            string total = totalTop + ";" + totalMore;
            string imgSize = imgTopWidth + "," + imgTopHeight + ";" + imgMoreWidth + "," + imgMoreHeight;

            for (int i = 0; i < checkListNewsConfig.Items.Count; i++)
            {
                if (checkListNewsConfig.Items[i].Selected)
                {
                    viewType += checkListNewsConfig.Items[i].Value + ",";
                }

            }
            if (viewType != "") viewType = viewType.Substring(0, viewType.Length - 1);
            else
            {
                lbMessage.Text = "Chọn loại tin cấu hình";
                return;
            }


            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Type.ToString(), viewType);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Total.ToString(), total);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Template.ToString(), dropTemplate.SelectedValue);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_ImgSize.ToString(), imgSize);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_SizeDes.ToString(), sizeDes);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_SizeTitle.ToString(), sizeTitle);
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

    }
}