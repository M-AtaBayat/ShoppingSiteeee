namespace ShoppingSiteManagement.Application.Contracts.OrderAPC
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double UnitPrice { get; set; }
        public int Count { get; set; }
        public double TotalItemPrice { get; set; }
    }
}
