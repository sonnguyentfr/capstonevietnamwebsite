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
using System.Threading;

namespace DesktopModules.TinTuc.ViewPage
{
    public partial class Setting : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private NV_NewsCategoriesController cateController = new NV_NewsCategoriesController();
        private NV_NewsTemplateController templateController = new NV_NewsTemplateController();
        private ModuleController moduleController = new ModuleController();
        private string _language;
        public override void LoadSettings()
        {
            try
            {
                BindTemplate();
                //tin hot
                if (!Null.IsNull(ModuleSettings[BL.settingListHot_PageSize.ToString()]))
                {
                    txtHot_PageSize.Text = ModuleSettings[BL.settingListHot_PageSize.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingList_ImgSizeHOT.ToString()]))
                {
                    string[] sImageSize = ModuleSettings[BL.settingList_ImgSizeHOT.ToString()].ToString().Split(';');
                    txtHot_ImgWidth.Text = sImageSize[0];
                    txtHot_ImgHeight.Text = sImageSize[1];
                }
                //==
                if (!Null.IsNull(ModuleSettings[BL.settingList_ImgSize.ToString()]))
                {
                    string[] sImageSize = ModuleSettings[BL.settingList_ImgSize.ToString()].ToString().Split(';');
                    txtList_ImgWidth.Text = sImageSize[0];
                    txtList_ImgHeight.Text = sImageSize[1];
                }
                if (!Null.IsNull(ModuleSettings[BL.settingList_Template.ToString()]))
                {
                    cbList_Template.SelectedValue = ModuleSettings[BL.settingList_Template.ToString()].ToString();
                }

                if (!Null.IsNull(ModuleSettings[BL.settingList_PageSize.ToString()]))
                {
                    txtList_PageSize.Text = ModuleSettings[BL.settingList_PageSize.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingDetails_Template]))
                {
                    cbDetails_Template.SelectedValue = ModuleSettings[BL.settingDetails_Template.ToString()].ToString();
                }
                //if (!Null.IsNull(ModuleSettings[BL.settingDetails_More.ToString()]))
                //{
                //    txtDetails_More.Text = ModuleSettings[BL.settingDetails_More.ToString()].ToString();
                //}
                if (!Null.IsNull(ModuleSettings[BL.settingDetails_Other.ToString()]))
                {
                    txtDetails_Other.Text = ModuleSettings[BL.settingDetails_Other.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingDetails_Comment.ToString()]))
                {
                    txtDetails_Cmt.Text = ModuleSettings[BL.settingDetails_Comment.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings["settingDetails_FBAppId"]))
                {
                    txt_commentFBId.Text = ModuleSettings["settingDetails_FBAppId"].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingDetails_Allow.ToString()]))
                {
                    string sAllow = ModuleSettings[BL.settingDetails_Allow.ToString()].ToString();
                    checkDetails_Cmt.Checked = Convert.ToBoolean(sAllow);
                }
                if (!Null.IsNull(ModuleSettings["settingDetails_AllowLogin"]))
                {
                    string sAllowLogin = ModuleSettings["settingDetails_AllowLogin"].ToString();
                    checkDetails_CmtLogin.Checked = Convert.ToBoolean(sAllowLogin);
                }
                if (!Null.IsNull(ModuleSettings["settingDetails_AllowFB"]))
                {
                    string sAllowFb = ModuleSettings["settingDetails_AllowFB"].ToString();
                    checkDetails_CmtFb.Checked = Convert.ToBoolean(sAllowFb);
                    if (sAllowFb == "True")
                    {
                        div_AllowCmtFBadmin.Visible = true;
                    }
                    else
                    {
                        div_AllowCmtFBadmin.Visible = false;
                    }
                }
                if (!Null.IsNull(ModuleSettings["settingDetails_FBAdmin"]))
                {
                    txt_commentFBadmin.Text = ModuleSettings["settingDetails_FBAdmin"].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingList_Order.ToString()]))
                {
                    cbList_Order.SelectedValue = ModuleSettings[BL.settingList_Order.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingList_SizeDes.ToString()]))
                {
                    txtList_SizeDes.Text = ModuleSettings[BL.settingList_SizeDes.ToString()].ToString();
                }
                if (!Null.IsNull(ModuleSettings[BL.settingList_ShowPage.ToString()]))
                {
                    checkList_ShowPage.Checked = Convert.ToBoolean(ModuleSettings[BL.settingList_ShowPage.ToString()]);
                }
                //Lay tin lien quan
                if (!Null.IsNull(ModuleSettings[BL.settingDetails_More.ToString()]))
                {
                    txttincungchuyenmucsoluong.Text = ModuleSettings[BL.settingDetails_More.ToString()].ToString();
                }
                string viewType = "CoDinh,Scroll";
                if (!Null.IsNull(ModuleSettings[BL.settingView_Type.ToString()]))
                {
                    viewType = ModuleSettings[BL.settingView_Type.ToString()].ToString();
                    if (viewType == "Scroll")
                    {
                        ShowViewTypeCate(true);
                        if (!Null.IsNull(ModuleSettings[BL.settingDetails_MorePage.ToString()]))
                        {
                            txttincungchuyenmucsotrang.Text = ModuleSettings[BL.settingDetails_MorePage.ToString()].ToString();
                        }
                    }
                    else
                    {
                        ShowViewTypeCate(false);
                    }
                }

            }
            catch { }
        }
        private void ShowViewTypeCate(bool viewCate)
        {
            rdGetType_Scroll.Checked = viewCate;
            div_rdGetType_Scroll.Visible = viewCate;
            rdGetType_Fix.Checked = !viewCate;
        }
        public override void UpdateSettings()
        {
            string listHOT_pageSize = txtHot_PageSize.Text != "" ? txtHot_PageSize.Text : "0";
            string list_imgWidthHot = txtHot_ImgWidth.Text != "" ? txtHot_ImgWidth.Text : "0";
            string list_imgHeightHot = txtHot_ImgHeight.Text != "" ? txtHot_ImgHeight.Text : "0";


            string list_pageSize = txtList_PageSize.Text != "" ? txtList_PageSize.Text : "0";
            string list_imgWidth = txtList_ImgWidth.Text != "" ? txtList_ImgWidth.Text : "0";
            string list_imgHeight = txtList_ImgHeight.Text != "" ? txtList_ImgHeight.Text : "0";
            string list_order = cbList_Order.SelectedValue;
            string list_sizeDes = txtList_SizeDes.Text != "" ? txtList_SizeDes.Text : "0";
            string list_showPage = checkList_ShowPage.Checked.ToString();

            string details_other = txtDetails_Other.Text != "" ? txtDetails_Other.Text : "0";
            string details_allowCmt = checkDetails_Cmt.Checked.ToString();
            string details_allowCmtLogin = checkDetails_CmtLogin.Checked.ToString();
            string details_allowCmtFb = checkDetails_CmtFb.Checked.ToString();
            string details_comment = txtDetails_Cmt.Text != "" ? txtDetails_Cmt.Text : "0";
            string details_FBAppId = txt_commentFBId.Text != "" ? txt_commentFBId.Text : "0";

            string details_FBAdmin = txt_commentFBadmin.Text != "" ? txt_commentFBadmin.Text : "";
            //tin lien quan
            string viewType = "";
            if (rdGetType_Fix.Checked)
            {
                viewType = "CoDinh";
            }
            else
            {
                viewType = "Scroll";
            }

            string details_more = txttincungchuyenmucsoluong.Text != "" ? txttincungchuyenmucsoluong.Text : "0";
            string details_morepage = txttincungchuyenmucsotrang.Text != "" ? txttincungchuyenmucsotrang.Text : "0";
            //---
            moduleController.UpdateModuleSetting(ModuleId, BL.settingListHot_PageSize.ToString(), listHOT_pageSize);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_ImgSizeHOT.ToString(), list_imgWidthHot + ";" + list_imgHeightHot);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_PageSize.ToString(), list_pageSize);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_ImgSize.ToString(), list_imgWidth + ";" + list_imgHeight);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_Template.ToString(), cbList_Template.SelectedValue);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_Order.ToString(), cbList_Order.SelectedValue);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_SizeDes.ToString(), list_sizeDes);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingList_ShowPage.ToString(), list_showPage);

            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_Template.ToString(), cbDetails_Template.SelectedValue);
            //Tin lien quan
            moduleController.UpdateModuleSetting(ModuleId, BL.settingView_Type.ToString(), viewType);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_More.ToString(), details_more);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_MorePage.ToString(), details_morepage);
            //--


            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_Other.ToString(), details_other);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_Allow.ToString(), details_allowCmt);
            moduleController.UpdateModuleSetting(ModuleId, "settingDetails_AllowLogin", details_allowCmtLogin);
            moduleController.UpdateModuleSetting(ModuleId, "settingDetails_AllowFB", details_allowCmtFb);
            moduleController.UpdateModuleSetting(ModuleId, BL.settingDetails_Comment.ToString(), details_comment);
            moduleController.UpdateModuleSetting(ModuleId, "settingDetails_FBAppId", details_FBAppId);
            moduleController.UpdateModuleSetting(ModuleId, "settingDetails_FBAdmin", details_FBAdmin);
            DataCache.ClearCache();
        }

        private void BindTemplate()
        {
            var list = templateController.GetTemplates(0);
            cbList_Template.DataSource = list;
            cbList_Template.DataTextField = "TemplateName";
            cbList_Template.DataValueField = "FilePath";
            cbList_Template.DataBind();
            if (!Null.IsNull(ModuleSettings[BL.settingList_Template.ToString()]))
            {
                cbList_Template.SelectedValue = ModuleSettings[BL.settingList_Template].ToString();
            }

            //Load template details

            cbDetails_Template.DataSource = list;
            cbDetails_Template.DataTextField = "TemplateName";
            cbDetails_Template.DataValueField = "FilePath";
            cbDetails_Template.DataBind();
            if (!Null.IsNull(ModuleSettings[BL.settingDetails_Template.ToString()]))
            {
                cbDetails_Template.SelectedValue = ModuleSettings[BL.settingDetails_Template].ToString();
            }
        }
        protected void checkDetails_CmtFb_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDetails_CmtFb.Checked)
            {
                div_AllowCmtFBadmin.Visible = true;
            }
            else
            {
                div_AllowCmtFBadmin.Visible = false;
            }
        }
        protected void rdGetType_CheckedChanged(object sender, EventArgs e)
        {
            if (rdGetType_Scroll.Checked)
            {
                div_rdGetType_Scroll.Visible = true;
            }
            if (rdGetType_Fix.Checked)
            {
                div_rdGetType_Scroll.Visible = false;
            }
        }
    }
}