namespace ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public string CreationDate { get; set; }
        public int ProductCount { get; set; }
    }
}
