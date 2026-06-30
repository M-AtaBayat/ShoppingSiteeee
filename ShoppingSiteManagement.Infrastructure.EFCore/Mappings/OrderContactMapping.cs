using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSiteManagement.Domain.OrderContactAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Mappings
{
    public class OrderContactMapping : IEntityTypeConfiguration<OrderContact>
    {
        public void Configure(EntityTypeBuilder<OrderContact> builder)
        {
            builder.ToTable("OrderContacts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.Property(x => x.TrackingCode).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        }
    }
}