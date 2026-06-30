using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Domain.AccountAgg;
using ShoppingSiteManagement.Domain.CartAgg;
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using ShoppingSiteManagement.Domain.ProductAgg;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;
using ShoppingSiteManagement.Domain.SettingsAgg;
using ShoppingSiteManagement.Infrastructure.EFCore.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Infrastructure.EFCore
{
    public class ShoppingSiteContext : DbContext
    {
        public ShoppingSiteContext(DbContextOptions<ShoppingSiteContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<OrderContact> OrderContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(ProductMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}