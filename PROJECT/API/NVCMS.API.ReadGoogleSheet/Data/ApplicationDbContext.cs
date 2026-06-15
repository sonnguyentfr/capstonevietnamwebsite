using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<student_from_ladipage> CrmDataLadings { get; set; }
        public DbSet<ZaloToken> Zalo_Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<student_from_ladipage>(entity =>
            {
                // Table
                entity.ToTable("student_from_ladipage");

                // Key
                entity.HasKey(e => e.id);

                // Columns (matching SQL schema)
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.hotendem)
                      .HasColumnName("hotendem")
                      .HasMaxLength(200);

                entity.Property(e => e.ten)
                      .HasColumnName("ten")
                      .HasMaxLength(50);

                entity.Property(e => e.gioi_tinh)
                      .HasColumnName("gioi_tinh");

                entity.Property(e => e.ngay_sinh)
                      .HasColumnName("ngay_sinh")
                      .HasColumnType("date");

                entity.Property(e => e.so_dien_thoai)
                      .HasColumnName("so_dien_thoai")
                      .HasMaxLength(30);

                entity.Property(e => e.email)
                      .HasColumnName("email")
                      .HasMaxLength(50);

                entity.Property(e => e.truong_dang_hoc)
                      .HasColumnName("truong_dang_hoc")
                      .HasMaxLength(500);

                entity.Property(e => e.event_dia_diem)
                      .HasColumnName("event_dia_diem")
                      .HasMaxLength(500);

                entity.Property(e => e.event_id)
                      .HasColumnName("event_id");

                entity.Property(e => e.event_dia_diem_id)
                      .HasColumnName("event_dia_diem_id");

                entity.Property(e => e.source)
                      .HasColumnName("source")
                      .HasMaxLength(500);

                entity.Property(e => e.medium)
                      .HasColumnName("medium")
                      .HasMaxLength(500);

                entity.Property(e => e.link)
                      .HasColumnName("link")
                      .HasMaxLength(500);

                entity.Property(e => e.ladi_page_id)
                      .HasColumnName("ladi_page_id")
                      .HasMaxLength(500);

                entity.Property(e => e.client_ip)
                      .HasColumnName("client_ip")
                      .HasMaxLength(50);

                // Optional extra column (if present in your class)
                entity.Property(e => e.thong_tin_khac)
                      .HasColumnName("thong_tin_khac")
                      .HasMaxLength(50);

                entity.Property(e => e.created_date)
                      .HasColumnName("created_date")
                      .HasColumnType("datetime");
                      // If you want EF to set default on insert when value not provided:
                      // .HasDefaultValueSql("'1970-01-01'")
            });
        }
    }
}