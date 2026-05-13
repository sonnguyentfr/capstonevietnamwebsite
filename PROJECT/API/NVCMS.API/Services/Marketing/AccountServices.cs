
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NVCMS.Modules.Marketing;

namespace NVCMS.API.Marketing.Services.Marketing
{
    public static class AccountServices
    {
        /// <summary>
        /// Lấy tất cả danh sách account
        /// </summary>
        public static List<Marketing_Mail_AccountInfo> GetAll()
        {
            var controller = new Marketing_Mail_AccountController();
            var arrayList = controller._GetAll();
            
            if (arrayList == null || arrayList.Count == 0)
            {
                return new List<Marketing_Mail_AccountInfo>();
            }

            return arrayList.Cast<Marketing_Mail_AccountInfo>().ToList();
        }

        /// <summary>
        /// Lấy account theo ID
        /// </summary>
        public static Marketing_Mail_AccountInfo GetById(int id)
        {
            var controller = new Marketing_Mail_AccountController();
            return controller._GetByID(id);
        }

        /// <summary>
        /// Lấy danh sách account theo UserId
        /// </summary>
        public static List<Marketing_Mail_AccountInfo> GetByUserId(int userId)
        {
            var allAccounts = GetAll();
            return allAccounts.Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// Lấy danh sách account theo PortalId
        /// </summary>
        public static List<Marketing_Mail_AccountInfo> GetByPortalId(int portalId)
        {
            var allAccounts = GetAll();
            return allAccounts.Where(x => x.PortalId == portalId).ToList();
        }

        /// <summary>
        /// Thêm mới account
        /// </summary>
        public static void Insert(string name, string mail, string password, int userId, int portalId)
        {
            var controller = new Marketing_Mail_AccountController();
            controller._Insert(name, mail, password, userId, portalId);
        }

        /// <summary>
        /// Thêm mới account từ model
        /// </summary>
        public static void Insert(Marketing_Mail_AccountInfo model)
        {
            var controller = new Marketing_Mail_AccountController();
            controller._Insert(model.Name, model.Mail, model.Password, model.UserId, model.PortalId);
        }

        /// <summary>
        /// Cập nhật account
        /// </summary>
        public static void Update(int id, string name, string mail, string password, int userId, int portalId)
        {
            var controller = new Marketing_Mail_AccountController();
            controller._Update(id, name, mail, password, userId, portalId);
        }

        /// <summary>
        /// Cập nhật account từ model
        /// </summary>
        public static void Update(Marketing_Mail_AccountInfo model)
        {
            var controller = new Marketing_Mail_AccountController();
            controller._Update(model.id, model.Name, model.Mail, model.Password, model.UserId, model.PortalId);
        }

        /// <summary>
        /// Xóa account theo ID
        /// </summary>
        public static void Delete(int id)
        {
            var controller = new Marketing_Mail_AccountController();
            controller._Delete(id);
        }

        /// <summary>
        /// Kiểm tra account có tồn tại không
        /// </summary>
        public static bool IsExist(int id)
        {
            var account = GetById(id);
            return account != null;
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        public static bool IsEmailExist(string email)
        {
            var allAccounts = GetAll();
            return allAccounts.Any(x => x.Mail != null && x.Mail.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa (exclude ID hiện tại khi update)
        /// </summary>
        public static bool IsEmailExist(string email, int excludeId)
        {
            var allAccounts = GetAll();
            return allAccounts.Any(x => x.id != excludeId && x.Mail != null && x.Mail.ToLower() == email.ToLower());
        }
    }
}