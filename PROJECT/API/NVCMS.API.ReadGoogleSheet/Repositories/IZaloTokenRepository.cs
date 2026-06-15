using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

public interface IZaloTokenRepository
{
    Task AddAsync(ZaloToken token);
    Task<ZaloToken> GetLastAsync();
}