using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.CartAPC;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHost.Pages
{
    [Authorize]
    public class BasketModel : PageModel
    {
        private readonly ICartApplication _cartApplication;

        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public double TotalPrice { get; set; }
        public int TotalItemsCount { get; set; }

        [TempData]
        public string StockErrorMessage { get; set; }

        public BasketModel(ICartApplication cartApplication)
        {
            _cartApplication = cartApplication;
        }

        public void OnGet()
        {
            var cart = _cartApplication.GetCart(User.Identity.Name);
            Items = cart.Items;
            TotalPrice = cart.TotalAmount;
            TotalItemsCount = cart.TotalItems;
        }

        public IActionResult OnPostIncrease(long id)
        {
            var result = _cartApplication.IncreaseItemCount(id);
            if (!result.IsSuccessed) StockErrorMessage = result.Message;
            return RedirectToPage();
        }

        public IActionResult OnPostDecrease(long id)
        {
            _cartApplication.DecreaseItemCount(id);
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(long id)
        {
            _cartApplication.RemoveFromCart(id);
            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            // ✅ فقط ریدایرکت به PaymentTransaction (بدون ایجاد سفارش)
            var cart = _cartApplication.GetCart(User.Identity.Name);
            if (cart == null || !cart.Items.Any())
            {
                StockErrorMessage = "سبد خرید خالی است";
                return RedirectToPage();
            }

            // ✅ اگر موجودی کافی است، برو فرم
            return RedirectToPage("/PaymentTransaction");
        }
    }
}