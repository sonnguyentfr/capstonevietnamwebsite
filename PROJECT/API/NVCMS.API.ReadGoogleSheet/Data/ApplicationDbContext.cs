using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Zalo_Token> Zalo_Token { get; set; }
        public DbSet<Zalo_Message_Log> Zalo_Message_Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            }
    }
}