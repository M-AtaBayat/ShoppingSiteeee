using _0_Framework.Domain;

namespace ShoppingSiteManagement.Domain.CartAgg
{
    public interface ICartRepository : IRepository<long, Cart>
    {
        Cart GetByAccountEmail(string email);
        void SaveChanges();
        Cart GetByItemId(long itemId);
        Cart GetActiveCartBy(string email);

    }
}
