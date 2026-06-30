using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using ShoppingSiteManagement.Domain.OrderAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
    {
        private readonly ShoppingSiteContext _context;
        public OrderRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public Order GetDetails(long id)
        {
            return _context.Orders
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Order> GetOrdersByAccountEmail(string email)
        {
            return _context.Orders
                .Include(x => x.Items)
                .Where(x => x.AccountEmail == email)
                .ToList();
        }

        public Order GetByTrackingCode(string trackingCode)
        {
            return _context.Orders
                .Include(x => x.Items)
                .FirstOrDefault(x => x.TrackingCode == trackingCode);
        }

        public Order GetByPostTrackingCode(string postTrackingCode)
        {
            return _context.Orders
                .Include(x => x.Items)
                .FirstOrDefault(x => x.PostTrackingCode == postTrackingCode);
        }
    }
}