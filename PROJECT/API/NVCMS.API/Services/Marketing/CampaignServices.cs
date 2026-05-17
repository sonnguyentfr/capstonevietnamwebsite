using System;
using System.Collections.Generic;
using System.Linq;
using NVCMS.Modules.Marketing;

namespace NVCMS.API.Marketing.Services.Marketing
{
    public static class CampaignServices
    {
        /// <summary>
        /// Lấy tất cả danh sách campaign (kèm số lượng email)
        /// </summary>
        public static List<Marketing_Mail_Campaing_ViewInfo> GetAll()
        {
            var controller = new Marketing_Mail_Campaing();
            var arrayList = controller._GetAll();

            if (arrayList == null || arrayList.Count == 0)
            {
                return new List<Marketing_Mail_Campaing_ViewInfo>();
            }

            return arrayList.Cast<Marketing_Mail_Campaing_ViewInfo>().ToList();
        }

        /// <summary>
        /// Lấy campaign theo ID
        /// </summary>
        public static Marketing_Mail_CampaingInfo GetById(int id)
        {
            var controller = new Marketing_Mail_Campaing();
            return controller._GetByID(id);
        }

        /// <summary>
        /// Lấy danh sách campaign theo UserId
        /// </summary>
        public static List<Marketing_Mail_Campaing_ViewInfo> GetByUserId(int userId)
        {
            return GetAll().Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// Lấy danh sách campaign theo PortalId
        /// </summary>
        public static List<Marketing_Mail_Campaing_ViewInfo> GetByPortalId(int portalId)
        {
            return GetAll().Where(x => x.PortalId == portalId).ToList();
        }

        /// <summary>
        /// Thêm mới campaign
        /// </summary>
        public static void Insert(string title, string description, DateTime createdDate, int userId, int portalId)
        {
            var controller = new Marketing_Mail_Campaing();
            controller._Insert(title, description, createdDate, userId, portalId);
        }

        /// <summary>
        /// Thêm mới campaign từ model
        /// </summary>
        public static void Insert(Marketing_Mail_CampaingInfo model)
        {
            var controller = new Marketing_Mail_Campaing();
            controller._Insert(model.Title, model.Description, model.CreatedDate, model.UserId, model.PortalId);
        }

        /// <summary>
        /// Cập nhật campaign
        /// </summary>
        public static void Update(int id, string title, string description, DateTime createdDate, int userId, int portalId)
        {
            var controller = new Marketing_Mail_Campaing();
            controller._Update(id, title, description, createdDate, userId, portalId);
        }

        /// <summary>
        /// Cập nhật campaign từ model
        /// </summary>
        public static void Update(Marketing_Mail_CampaingInfo model)
        {
            var controller = new Marketing_Mail_Campaing();
            controller._Update(model.id, model.Title, model.Description, model.CreatedDate, model.UserId, model.PortalId);
        }

        /// <summary>
        /// Xóa campaign theo ID
        /// </summary>
        public static void Delete(int id)
        {
            var controller = new Marketing_Mail_Campaing();
            controller._Delete(id);
        }

        /// <summary>
        /// Kiểm tra campaign có tồn tại không
        /// </summary>
        public static bool IsExist(int id)
        {
            return GetById(id) != null;
        }

        /// <summary>
        /// Kiểm tra title đã tồn tại chưa
        /// </summary>
        public static bool IsTitleExist(string title)
        {
            return GetAll().Any(x => x.Title != null && x.Title.ToLower() == title.ToLower());
        }

        /// <summary>
        /// Kiểm tra title đã tồn tại chưa (exclude ID hiện tại khi update)
        /// </summary>
        public static bool IsTitleExist(string title, int excludeId)
        {
            return GetAll().Any(x => x.id != excludeId && x.Title != null && x.Title.ToLower() == title.ToLower());
        }
    }
}