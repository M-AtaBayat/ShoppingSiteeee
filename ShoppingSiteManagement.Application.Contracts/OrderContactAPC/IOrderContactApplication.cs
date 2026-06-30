using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.OrderContactAPC
{
    public interface IOrderContactApplication
        {
            OperationResult Register(RegisterOrderContact command);
            OperationResult MarkAsRead(long id);
            List<OrderContactViewModel> GetList();
        }
}