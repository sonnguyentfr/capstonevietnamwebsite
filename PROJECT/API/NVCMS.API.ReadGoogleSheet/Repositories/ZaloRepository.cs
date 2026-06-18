using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;

public class ZaloTokenRepository : IZaloTokenRepository
{
    private readonly ApplicationDbContext _context;

    public ZaloTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Zalo_Token token)
    {
        _context.Zalo_Token.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<Zalo_Token> GetLastAsync()
    {
        try
        {
            var token = await _context.Zalo_Token.OrderByDescending(z => z.Id).FirstOrDefaultAsync();
            if (token == null)
                throw new InvalidOperationException("No ZaloToken found.");
            return token;
        }
        catch (Exception ex)
        {
            throw ex;
        } 

    }
}