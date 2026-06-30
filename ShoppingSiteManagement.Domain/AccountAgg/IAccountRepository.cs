using _0_Framework.Domain;

namespace ShoppingSiteManagement.Domain.AccountAgg
{
    public interface IAccountRepository : IRepository<long, Account>
    {
        Account GetByEmail(string email);
    }
}
