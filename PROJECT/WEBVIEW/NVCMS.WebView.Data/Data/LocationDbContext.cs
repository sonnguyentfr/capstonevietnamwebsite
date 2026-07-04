using Microsoft.EntityFrameworkCore;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Data;

public class LocationDbContext : DbContext
{
    public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options) { }

    public DbSet<CapLocationModel> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CapLocationModel>(e =>
        {
            e.ToTable("Cap_Location");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            e.Property(x => x.Name).HasColumnName("Name").HasMaxLength(255);
            e.Property(x => x.ShortName).HasColumnName("ShortName").HasMaxLength(255);
            e.Property(x => x.ParentId).HasColumnName("ParentId");
            e.Property(x => x.Status).HasColumnName("Status");
            e.Property(x => x.Ordernumber).HasColumnName("Ordernumber");
            e.Property(x => x.PortalId).HasColumnName("PortalId");
        });
    }
}
