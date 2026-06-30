namespace ShoppingSiteManagement.Application.Contracts.ProductAPC
{
    public class ProductSearchModel
    {
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public bool HasDiscount { get; set; }
        public bool IsPopular { get; set; }
    }
}
