using _01_ShoppingSiteQuery.Contracts.Product;
using _01_ShoppingSiteQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ServiceHost.Pages
{
    public class ProductsModel : PageModel
    {
        public List<ProductQueryModel> Products { get; set; }
        public List<ProductCategoryQueryModel> Categories { get; set; }

        private readonly IProductQuery _productQuery;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductsModel(IProductQuery productQuery, IProductCategoryQuery productCategoryQuery)
        {
            _productQuery = productQuery;
            _productCategoryQuery = productCategoryQuery;
        }

        public void OnGet()
        {
            Products = _productQuery.GetLatestProducts();
            Categories = _productCategoryQuery.GetProductCategories();
        }
    }
}