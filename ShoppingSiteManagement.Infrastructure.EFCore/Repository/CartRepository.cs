using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Domain.CartAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class CartRepository : RepositoryBase<long, Cart>, ICartRepository
    {
        private readonly ShoppingSiteContext _context;
        public CartRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public Cart GetActiveCartBy(string email)
        {
            return _context.Carts
                .Include(x => x.Items).Where(x => x.IsDeleted == false)
                .FirstOrDefault(x => x.AccountEmail == email && !x.IsFinished);
        }

        public Cart GetByItemId(long itemId)
        {
            return _context.Carts
                .Include(x => x.Items).Where(x=>x.IsDeleted == false)
                .FirstOrDefault(x => x.Items.Any(z => z.Id == itemId));
        }


        public Cart GetByAccountEmail(string email)
        {
            return _context.Carts.Include(x => x.Items).Where(x => x.IsDeleted == false).FirstOrDefault(x => x.AccountEmail == email);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}