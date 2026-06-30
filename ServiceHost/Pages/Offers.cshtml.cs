using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class OffersModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        public List<ProductQueryModel> OfferedProducts { get; set; }
        public OffersModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet()
        {
            OfferedProducts = _productQuery.GetProductsWithDiscount().ToList();
        }
    }
}
