using System;
using System.Collections.Generic;
using System.Linq;
using NVCMS.API.Model.Marketing;
using NVCMS.Modules.Marketing;

namespace NVCMS.API.Marketing.Services.Marketing
{
    public static class ZaloCampaignServices
    {
        public static List<Marketing_Zalo_CampaignInfo> GetAll(int portalId)
        {
            var ctl = new Marketing_Zalo_Campaign_Controller();
            var arr = ctl._GetAll(portalId);
            if (arr == null || arr.Count == 0) return new List<Marketing_Zalo_CampaignInfo>();
            return arr.Cast<Marketing_Zalo_CampaignInfo>().ToList();
        }

        public static Marketing_Zalo_CampaignInfo GetById(int id)
        {
            var ctl = new Marketing_Zalo_Campaign_Controller();
            return ctl._GetByID(id);
        }

        public static int Insert(Marketing_Zalo_CampaignInfo model)
        {
            var ctl = new Marketing_Zalo_Campaign_Controller();
            return ctl._Insert(model.Title, model.Description, model.Status, DateTime.Now, model.UserId, model.PortalId);
        }

        public static void Update(Marketing_Zalo_CampaignInfo model)
        {
            var ctl = new Marketing_Zalo_Campaign_Controller();
            ctl._Update(model.Id, model.Title, model.Description, model.Status, model.UserId, model.PortalId);
        }

        public static void Delete(int id)
        {
            var ctl = new Marketing_Zalo_Campaign_Controller();
            ctl._Delete(id);
        }

        public static List<ZnsTemplateInfoDto> GetZnsTemplates()
        {
            var ctl = new Marketing_ZNS_TemplateController();
            var templates = ctl._GetAll();

            if (templates == null || templates.Count == 0)
                return new List<ZnsTemplateInfoDto>();

            var result = new List<ZnsTemplateInfoDto>(templates.Count);
            foreach (var item in templates.Cast<Marketing_ZNS_TemplateInfo>())
            {
                if (item == null) continue;

                result.Add(new ZnsTemplateInfoDto
                {
                    Id = item.Id,
                    TemplateId = item.TemplateId,
                    TemplateName = item.TemplateName,
                    Status = item.Status,
                    PreviewUrl = item.PreviewUrl,
                    IsActive = item.IsActive,
                    UpdatedAt = item.UpdatedAt
                });
            }

            return result;
        }

        public static ZnsTemplateInfoDto GetZnsTemplateByTemplateId(long templateId)
        {
            var ctl = new Marketing_ZNS_TemplateController();
            var item = ctl._GetByTemplateId(templateId);
            if (item == null) return null;

            return new ZnsTemplateInfoDto
            {
                Id = item.Id,
                TemplateId = item.TemplateId,
                TemplateName = item.TemplateName,
                Status = item.Status,
                PreviewUrl = item.PreviewUrl,
                IsActive = item.IsActive,
                UpdatedAt = item.UpdatedAt
            };
        }
    }

    public static class ZaloListSdtServices
    {
        public static string NormalizePhone(string raw) => ZaloPhoneHelper.NormalizePhone(raw);
        public static bool IsValidPhone(string phone) => ZaloPhoneHelper.IsValidPhone(phone);

        public static int AddPhone(int campaignId, string fullname, string phoneRaw, int userId, int portalId, out string errorMsg)
        {
            errorMsg = string.Empty;
            string normalized = null;
            normalized = ZaloPhoneHelper.ValidateAndNormalize(phoneRaw, ref errorMsg);
            if (normalized == null) return -2;

            var ctl = new Marketing_Zalo_ListSdt_Controller();
            return ctl._Insert(campaignId, fullname, phoneRaw, normalized, 0, DateTime.Now, userId, portalId);
        }

        public static Marketing_Zalo_ListSdt_BulkResult AddBulk(int campaignId, string rawPhoneList, int userId, int portalId)
        {
            var validList = new List<string>();
            var invalidList = new List<string>();
            ZaloPhoneHelper.ProcessPhoneList(rawPhoneList, ref validList, ref invalidList);

            if (validList.Count == 0)
                return new Marketing_Zalo_ListSdt_BulkResult { InsertCount = 0, DupCount = 0 };

            var csv = string.Join(",", validList);
            var ctl = new Marketing_Zalo_ListSdt_Controller();
            return ctl._InsertBulk(campaignId, csv, userId, portalId);
        }

        public static ZaloPhoneValidateResult ValidatePhoneList(string rawPhoneList)
        {
            var validList = new List<string>();
            var invalidList = new List<string>();
            ZaloPhoneHelper.ProcessPhoneList(rawPhoneList, ref validList, ref invalidList);

            var unique = validList.Distinct().ToList();
            int dupInBatch = validList.Count - unique.Count;

            return new ZaloPhoneValidateResult
            {
                ValidCount = unique.Count,
                InvalidCount = invalidList.Count,
                DupCount = dupInBatch,
                ValidList = unique,
                InvalidList = invalidList
            };
        }

        public static List<Marketing_Zalo_ListSdtInfo> GetAll(int campaignId, string keySearch = "", int status = -1, int pageIndex = 0, int pageSize = 50)
        {
            var ctl = new Marketing_Zalo_ListSdt_Controller();
            var arr = ctl._GetAll(campaignId, keySearch, status, pageIndex, pageSize);
            if (arr == null || arr.Count == 0) return new List<Marketing_Zalo_ListSdtInfo>();
            return arr.Cast<Marketing_Zalo_ListSdtInfo>().ToList();
        }

        public static void Delete(int id)
        {
            new Marketing_Zalo_ListSdt_Controller()._Delete(id);
        }

        public static void DeleteByCampaignId(int campaignId)
        {
            new Marketing_Zalo_ListSdt_Controller()._DeleteByCampaignId(campaignId);
        }
    }
}
