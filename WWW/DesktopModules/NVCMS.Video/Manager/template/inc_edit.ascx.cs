using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Entities;
using NVCMS.Modules.Video;
using System.IO;
using DotNetNuke.Common;
namespace DesktopModules.TinTuc.Manager.template
{
    public partial class inc_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        #region URL Friendly 

        private static readonly string[] VietNamChar = new string[] { "aeouidy", "áàạảãâấầậẩẫăắằặẳẵ","éèẹẻẽêếềệểễ",
  "óòọỏõôốồộổỗơớờợởỡ", "úùụủũưứừựửữ", "íìịỉĩ","đ", "ýỳỵỷỹ", };
        public static string TrimVietnamesChar(string str)
        {
            string tg = str.ToLower().Trim();
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    tg = tg.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return tg;
        }
        public static string ToUrlFriendly(string strInput)
        {
            string strTitle = strInput.Trim();
            strTitle = strTitle.Trim('-');

            char[] chars = @"–$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            strTitle = strTitle.Replace("c#", "C-Sharp");
            strTitle = strTitle.Replace("vb.net", "VB-Net");
            strTitle = strTitle.Replace("asp.net", "Asp-Net");

            //Loc dau tieng viet
            strTitle = TrimVietnamesChar(strTitle);

            //Thay dau (.) bang dau (-)
            strTitle.Replace(".", "-");

            //Thay ky tu dac biet bang dau (-)
            for (int i = 0; i < chars.Length; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (strTitle.Contains(strChar))
                {
                    strTitle = strTitle.Replace(strChar, "-");
                }
            }
            //Thay the tat cac khoang trang bang 1 dau (-)
            strTitle = strTitle.Replace(" ", "-");

            //Thay the nhieu dau (-) bang 1 dau (-)
            strTitle = strTitle.Replace("--", "-");
            strTitle = strTitle.Replace("---", "-");
            strTitle = strTitle.Replace("----", "-");
            strTitle = strTitle.Replace("-----", "-");
            strTitle = strTitle.Replace("-----", "-");
            strTitle = strTitle.Replace("---", "-");
            strTitle = strTitle.Replace("--", "-");

            //Trim Start and End Spaces
            strTitle = strTitle.Trim();

            //Trim "-" Hyphen 
            strTitle = strTitle.Trim('-');

            return strTitle;
        }

        #endregion URL Friendly

        private string PageListURL = "/quan-tri/video-cap-cao/quan-tri-template";
        private string folderPath;

        Video_TemplateController controller = new Video_TemplateController();
        protected void Page_Load(object sender, EventArgs e)
        {
            folderPath = PortalSettings.HomeDirectoryMapPath + "VideoTemplate";
            // Response.Write(folderPath);

            if (Request.QueryString["id"] == null) ltTitle.Text = "Thêm mới Template";
            else { ltTitle.Text = "Cập nhật Template"; FillData(); }
        }

        private void FillData()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            Video_TemplateInfo template = controller.GetTemplate(PortalId, id);
            if (template == null) return;
            txtTemplateName.Value = template.TemplateName;
            txtFilePath.Value = PortalSettings.HomeDirectory + "/VideoTemplate/" + template.FilePath;
            if (File.Exists(folderPath + "/" + template.FilePath))
            {
                txtValue.Value = File.ReadAllText(folderPath + "/" + template.FilePath);
                hdf_textcode.Value = File.ReadAllText(folderPath + "/" + template.FilePath);
            }
            else
            {
                txtValue.Value = "File template không tồn tại. Vui lòng cập nhật lại !";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

            int id = Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : -1;
            Video_TemplateInfo template = (id == -1 ? new Video_TemplateInfo() : controller.GetTemplate(PortalId, id));
            template.TemplateName = txtTemplateName.Value;
            string fileName = ToUrlFriendly(template.TemplateName + ".html");
            template.PortalID = PortalId;
            if (id == -1)
            {
                template.FilePath = fileName;
                controller.InsertTemplate(template.TemplateName, template.FilePath, template.PortalID);
                File.WriteAllText(folderPath + "/" + template.FilePath, hdf_textcode.Value);
                lbMessage.Text = "Tempalte đã được thêm mới";
            }
            else
            {
                File.WriteAllText(folderPath + "/" + template.FilePath, hdf_textcode.Value);
                controller.UpdateTemplate(id, template.TemplateName, template.FilePath);
                lbMessage.Text = "Tempalte đã được cập nhật";
            }
            FillData();
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("/",true);
        }
    }
}