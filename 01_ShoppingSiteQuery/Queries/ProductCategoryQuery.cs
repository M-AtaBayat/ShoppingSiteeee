using _01_ShoppingSiteQuery.Contracts.ProductCategory;
using ShoppingSiteManagement.Infrastructure.EFCore;

namespace _01_ShoppingSiteQuery.Queries
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShoppingSiteContext _context; 

        public ProductCategoryQuery(ShoppingSiteContext context)
        {
            _context = context;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _context.ProductCategories
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
        }
    }
}