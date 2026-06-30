using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;
        public LatestProductsViewComponent(IProductQuery productQuery) => _productQuery = productQuery;

        public IViewComponentResult Invoke()
        {
            var products = _productQuery.GetLatestProducts();
            return View(products);
        }
    }
}