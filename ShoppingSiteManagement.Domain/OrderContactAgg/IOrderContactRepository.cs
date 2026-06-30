using _0_Framework.Domain;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;

namespace ShoppingSiteManagement.Domain.OrderContactAgg
{
    public interface IOrderContactRepository : IRepository<long, OrderContact>
    {
        List<OrderContactViewModel> GetList();
    }
}