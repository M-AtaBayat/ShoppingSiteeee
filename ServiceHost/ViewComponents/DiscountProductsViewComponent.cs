using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class DiscountProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;
        public DiscountProductsViewComponent(IProductQuery productQuery) => _productQuery = productQuery;

        public IViewComponentResult Invoke()
        {
            var products = _productQuery.GetProductsWithDiscount();
            return View(products);
        }
    }
}