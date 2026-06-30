using _0_Framework.Infrastructure;
using ShoppingSiteManagement.Domain.SettingsAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class SiteSettingsRepository : RepositoryBase<int, SiteSettings>, ISiteSettingsRepository
    {
        private readonly ShoppingSiteContext _context;
        public SiteSettingsRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }
    }
}