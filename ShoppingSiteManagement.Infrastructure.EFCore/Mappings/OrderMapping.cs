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
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TrackingCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PostTrackingCode).HasMaxLength(100);

            builder.Property(x => x.TotalProductsPrice).HasPrecision(18, 2);
            builder.Property(x => x.ShippingCost).HasPrecision(18, 2);
            builder.Property(x => x.FinalAmount).HasPrecision(18, 2);

            // ✅ تغییر: این فیلدها Required نیستند (بعداً تکمیل می‌شوند)
            builder.Property(x => x.Province).HasMaxLength(100);
            builder.Property(x => x.City).HasMaxLength(100);

            builder.Property(x => x.AccountEmail).IsRequired().HasMaxLength(250);
            builder.Property(x => x.ReceiverName).HasMaxLength(100);           // ✅ حذف IsRequired()
            builder.Property(x => x.ReceiverPhoneNumber).HasMaxLength(20);     // ✅ حذف IsRequired()
            builder.Property(x => x.Address).HasMaxLength(500);               // ✅ حذف IsRequired()
            builder.Property(x => x.PostalCode).HasMaxLength(20);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}