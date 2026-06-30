using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.CartAPC
{
    public interface ICartApplication
    {
        OperationResult ReleaseReservedStock(string accountEmail);
        OperationResult AddToCart(long productId, string accountEmail);
        OperationResult RemoveFromCart(long itemId);
        OperationResult IncreaseItemCount(long itemId);
        OperationResult DecreaseItemCount(long itemId);
        CartViewModel GetCart(string accountEmail);
        OperationResult Checkout(string accountEmail);
        OperationResult ReleaseExpiredCarts();
    }
}
