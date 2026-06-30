using _0_Framework.Infrastructure;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class ProductCategoryRepository : RepositoryBase<int, ProductCategory>, IProductCategoryRepository
    {
        private readonly ShoppingSiteContext _context;
        public ProductCategoryRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public EditProductCategory GetDetails(int id)
        {
            return _context.ProductCategories
                .Where(x => x.Id == id)
                .Select(x => new EditProductCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category
                }).FirstOrDefault()!;
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            var query = _context.ProductCategories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return query.Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                IsDeleted = x.IsDeleted,
                CreationDate = x.CreationDate.ToString("yyyy/MM/dd HH:mm:ss"),
                ProductCount = x.Products.Count
            }).ToList();
        }
    }
}