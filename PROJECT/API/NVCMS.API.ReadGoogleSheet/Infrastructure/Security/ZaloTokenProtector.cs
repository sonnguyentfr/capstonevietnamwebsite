using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure.Security
{
    public interface IZaloTokenProtector
    {
        /// <summary>Mã hoá token để lưu DB. Kết quả có tiền tố nhận diện.</summary>
        string Protect(string plainText);

        /// <summary>Giải mã token đọc từ DB. Token cũ lưu dạng plaintext được trả nguyên.</summary>
        string Unprotect(string storedValue);

        bool IsProtected(string? storedValue);
    }

    /// <summary>
    /// Mã hoá token Zalo bằng ASP.NET Core Data Protection.
    /// Key ring lưu ở thư mục cấu hình "DataProtection:KeysPath" - PHẢI giữ nguyên thư mục này khi deploy,
    /// mất key thì phải lấy lại token qua POST /api/Zalo/get-access-token.
    /// </summary>
    public class ZaloTokenProtector : IZaloTokenProtector
    {
        public const string Prefix = "dp1:";
        private readonly IDataProtector _protector;

        public ZaloTokenProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("NVCMS.ZaloToken.v1");
        }

        public bool IsProtected(string? storedValue)
            => storedValue != null && storedValue.StartsWith(Prefix, StringComparison.Ordinal);

        public string Protect(string plainText)
        {
            if (string.IsNullOrEmpty(plainText) || IsProtected(plainText))
                return plainText;
            return Prefix + _protector.Protect(plainText);
        }

        public string Unprotect(string storedValue)
        {
            if (!IsProtected(storedValue))
                return storedValue;
            try
            {
                return _protector.Unprotect(storedValue[Prefix.Length..]);
            }
            catch (CryptographicException ex)
            {
                throw new Common.ZaloTokenUnavailableException(
                    "Không giải mã được Zalo token (key Data Protection đã thay đổi/mất). " +
                    "Cần lấy lại token qua POST /api/Zalo/get-access-token.", ex);
            }
        }
    }
}
