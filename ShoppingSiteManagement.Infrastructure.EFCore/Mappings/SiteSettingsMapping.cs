using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSiteManagement.Domain.SettingsAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Mappings
{
    public class SiteSettingsMapping : IEntityTypeConfiguration<SiteSettings>
    {
        public void Configure(EntityTypeBuilder<SiteSettings> builder)
        {
            builder.ToTable("SiteSettings");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ShippingCost).HasPrecision(18, 2);
            builder.Property(x => x.AdminEmail).IsRequired().HasMaxLength(250);
        }
    }
}