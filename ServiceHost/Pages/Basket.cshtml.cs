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
            var result = _cartApplication.Checkout(User.Identity.Name);
            if (result.IsSuccessed)
            {
                // 🟢 تغییر مهم: هدایت به صفحه پرداخت همراه با شناسه سفارش یا پیغام موفقیت
                // اگر متد Checkout شما در خروجی (result) شناسه سفارش رو میده، از اون استفاده کن، در غیر این صورت معمولی برو ولی با آگاهی از وضعیت دیتابیس
                return RedirectToPage("/PaymentTransaction");
            }

            StockErrorMessage = result.Message;
            return RedirectToPage();
        }
    }
}
