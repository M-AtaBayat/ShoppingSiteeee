namespace _01_ShoppingSiteQuery.Contracts.Product
{
    public class ProductQueryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
        public string PriceWithDiscount { get; set; }
        public bool HasDiscount { get; set; }
        public int DiscountRate { get; set; }
        public bool IsPopular { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }
        public int StockCount { get; set; }
        public string Description { get; set; }
        public string Picture1 { get; set; }
        public string Size { get; set; }
        public string MetaDescription { get; set; }
        public string Color { get; set; }
        public string Keywords { get; set; }
        public string Mark { get; set; }
        public string Picture2 { get; set; }
    }
}