using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

public interface IZaloService
{
    Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code);
    Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refresh_token);
    Task<Zalo_Token> GetLastTokenAsync();
    Task<ZaloMessageResponse> SendTemplateAsync<T>(ZaloMessageRequest<T> request);
}