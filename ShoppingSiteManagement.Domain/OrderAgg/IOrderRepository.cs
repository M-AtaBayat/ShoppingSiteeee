
using _0_Framework.Domain;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;

namespace ShoppingSiteManagement.Domain.OrderAgg
{
    public interface IOrderRepository : IRepository<long, Order>
    {
        List<Order> GetOrdersByAccountEmail(string email);
        Order GetByTrackingCode(string trackingCode);
        Order GetByPostTrackingCode(string postTrackingCode);
        Order GetDetails(long id);
    }
}
