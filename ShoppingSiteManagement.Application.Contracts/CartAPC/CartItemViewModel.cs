namespace ShoppingSiteManagement.Application.Contracts.CartAPC
{
    public class CartItemViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double UnitPrice { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public bool HasDiscount { get; set; }
        public double DiscountedPrice { get; set; }
        public bool IsDeleted { get; set; }
        public int DiscountRate { get; set; }
        public int StockCount { get; set; }
    }
}
