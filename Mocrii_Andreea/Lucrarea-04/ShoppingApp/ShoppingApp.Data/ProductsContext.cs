using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data.Models;
using System.Linq;

namespace ShoppingApp.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base (options)
        {

        }
        public DbSet<OrderLineDto> OrderLines { get; set; }

        public DbSet<OrderHeaderDto> OrderHeaders { get; set; }


        public DbSet<ProductDto> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(p => p.ProductId);
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLine").HasKey(ol => ol.OrderId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeader").HasKey(oh => oh.OrderId);
        }
    }
}