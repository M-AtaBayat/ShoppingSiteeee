using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSiteManagement.Domain.OrderAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Mappings
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ProductImage).HasMaxLength(500);

            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        }
    }
}