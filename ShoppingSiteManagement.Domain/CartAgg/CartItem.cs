using _0_Framework.Domain;

namespace ShoppingSiteManagement.Domain.CartAgg
{
    public class CartItem : EntityBase<long>
    {
        public long CartId { get; private set; }
        public long ProductId { get; private set; }
        public int Count { get; private set; }
        public double UnitPrice { get; private set; }

        public double TotalPrice => UnitPrice * Count;

        protected CartItem() { }

        public CartItem(long cartId, long productId, int count, double unitPrice)
        {
            CartId = cartId;
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
        }
        public void Increase(int count)
        {
            Count += count;
        }
        public void Decrease(int count)
        {
            Count -= count;
        }
    }
}
