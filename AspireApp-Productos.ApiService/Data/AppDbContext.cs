using Microsoft.EntityFrameworkCore;
using AspireApp_Productos.ApiService.Models;

namespace AspireApp_Productos.ApiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Products");
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
                builder.Property(p => p.Description).HasMaxLength(1000);
                builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
                builder.Property(p => p.CreatedAt).IsRequired();
            });
        }
    }
}
