using _0_Framework.Infrastructure;
using ShoppingSiteManagement.Domain.AccountAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
    {
        private readonly ShoppingSiteContext _context;
        public AccountRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public Account GetByEmail(string email)
        {
            return _context.Accounts.FirstOrDefault(x => x.Email == email);
        }
    }
}