using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

public interface IZaloTokenRepository
{
    Task AddAsync(Zalo_Token token);
    Task<Zalo_Token> GetLastAsync();
}