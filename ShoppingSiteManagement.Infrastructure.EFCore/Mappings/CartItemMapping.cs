using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSiteManagement.Domain.CartAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Mappings
{
    public class CartItemMapping : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        }
    }
}