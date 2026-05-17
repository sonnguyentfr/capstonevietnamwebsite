using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace NVCMS.Modules.School
{
    public partial class SearchSetting : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private ModuleController moduleController = new ModuleController();
        public override void LoadSettings()
        {
            try
            {
                lbMessage.Text = ModuleId.ToString();
                string viewType = "normal,major";
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

            }
            catch { }
        }

        public override void UpdateSettings()
        {

            string viewType = "";

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

            DataCache.ClearCache();
        }


    }
}