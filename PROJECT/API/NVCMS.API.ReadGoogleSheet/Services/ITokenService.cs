namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}