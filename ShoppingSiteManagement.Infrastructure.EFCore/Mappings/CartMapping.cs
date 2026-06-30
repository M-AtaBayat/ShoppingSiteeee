using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSiteManagement.Domain.CartAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Mappings
{
    public class CartMapping : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountEmail).IsRequired().HasMaxLength(250);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
            builder.Property(x => x.PayableAmount).HasPrecision(18, 2);

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}