using _0_Framework.Domain;

namespace ShoppingSiteManagement.Domain.OrderAgg
{
    public class OrderItem : EntityBase<long>
    {
        public long OrderId { get; private set; }
        public Order Order { get; private set; }
        public long ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string ProductImage { get; private set; }
        public double UnitPrice { get; private set; }
        public int Count { get; private set; }
        public double TotalItemPrice => UnitPrice * Count;

        protected OrderItem() { }

        public OrderItem(long orderId, long productId, string productName, string productImage, double unitPrice, int count)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ProductImage = productImage;
            UnitPrice = unitPrice;
            Count = count;
        }
    }
}
