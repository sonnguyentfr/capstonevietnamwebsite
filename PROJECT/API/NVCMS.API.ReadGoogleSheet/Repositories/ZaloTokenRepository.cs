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

    public async Task AddAsync(ZaloToken token)
    {
        _context.Zalo_Token.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<ZaloToken> GetLastAsync()
    {
        var token = await _context.Zalo_Token.OrderByDescending(z => z.CreatedAt).FirstOrDefaultAsync();
        if (token == null)
            throw new InvalidOperationException("No ZaloToken found.");
        return token;
    }
}