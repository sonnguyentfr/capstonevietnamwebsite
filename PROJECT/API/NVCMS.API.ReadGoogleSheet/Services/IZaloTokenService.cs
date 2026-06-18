using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

public interface IZaloTokenService
{
    Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code);
    Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refresh_token);
    Task<Zalo_Token> GetLastTokenAsync();
}