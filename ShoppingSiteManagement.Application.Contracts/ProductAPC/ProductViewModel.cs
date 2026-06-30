namespace ShoppingSiteManagement.Application.Contracts.ProductAPC
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrginalImage { get; set; }
        public string MoreImage1 { get; set; }
        public string MoreImage2 { get; set; }
        public string Size { get; set; }
        public string Mark { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
        public bool HasDiscount { get; set; }
        public double? DiscountedPrice { get; set; }
        public bool IsPopular { get; set; }
        public string Slug { get; set; }
        public string Keywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsDeleted { get; set; }
        public string CreationDate { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
    }
}
