using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.CartAPC;

namespace ServiceHost.Pages
{
    public class ProductDetailModel : PageModel
    {
        public ProductQueryModel Product { get; set; }
        public int CountInCart { get; set; }
        private readonly IProductQuery _productQuery;
        private readonly ICartApplication _cartApplication;
        public ProductDetailModel(IProductQuery productQuery, ICartApplication cartApplication)
        {
            _productQuery = productQuery;
            _cartApplication = cartApplication;
        }

        public IActionResult OnGet(string slug)
        {
            Product = _productQuery.GetProductDetails(slug);

            if (Product == null)
                return RedirectToPage("./Index");

            if (User.Identity.IsAuthenticated)
            {
                var cart = _cartApplication.GetCart(User.Identity.Name);
                var item = cart.Items.FirstOrDefault(x=>x.ProductId == Product.Id);
                CountInCart = item?.Count ?? 0;
            }

            ViewData["Title"] = Product.Name;
            ViewData["Keywords"] = Product.Keywords;
            ViewData["MetaDescription"] = Product.MetaDescription;

            return Page();
        }
        public IActionResult OnPostAddToCart(long productId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

            var accountEmail = User.Identity.Name;
            var result = _cartApplication.AddToCart(productId, accountEmail);

            return RedirectToPage();
        }
    }
}
