using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class PopularProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;
        public PopularProductsViewComponent(IProductQuery productQuery) => _productQuery = productQuery;

        public IViewComponentResult Invoke()
        {
            var products = _productQuery.GetPopularProducts();
            return View(products);
        }
    }
}