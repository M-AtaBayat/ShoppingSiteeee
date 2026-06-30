using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.CartAPC;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using ShoppingSiteManagement.Application.Contracts.SettingsAPC;

namespace ServiceHost.Pages
{
    [Authorize]
    public class PaymentTransactionModel : PageModel
    {
        private readonly ICartApplication _cartApplication;
        private readonly IOrderApplication _orderApplication;
        private readonly ISettingsApplication _settingsApplication;

        public PaymentTransactionModel(ICartApplication cartApplication, IOrderApplication orderApplication, ISettingsApplication settingsApplication)
        {
            _cartApplication = cartApplication;
            _orderApplication = orderApplication;
            _settingsApplication = settingsApplication;
        }

        [BindProperty]
        public CheckoutDto CheckoutInfo { get; set; }

        public int TotalItemsCount { get; set; }
        public double CartAmount { get; set; }
        public double ShippingCost { get; set; }
        public double FinalTotalAmount { get; set; }

        public IActionResult OnGet()
        {
            var cart = _cartApplication.GetCart(User.Identity.Name);

            if (cart == null || !cart.Items.Any())
                return RedirectToPage("/Basket");

            var settings = _settingsApplication.GetSettings();
            var shippingCost = settings?.ShippingCost ?? 0;

            TotalItemsCount = cart.TotalItems;
            CartAmount = cart.TotalAmount;
            ShippingCost = shippingCost;
            FinalTotalAmount = CartAmount + ShippingCost;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            // ✅ Validate کامل
            if (string.IsNullOrWhiteSpace(CheckoutInfo?.Name) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Phone) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Province) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.City) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Address))
            {
                TempData["Error"] = "لطفاً تمام فیلدها را پر کنید";
                return RedirectToPage();
            }

            // ✅ هزینه ارسال را از SiteSettings بخون
            var settings = _settingsApplication.GetSettings();
            var shippingCost = settings?.ShippingCost ?? 0;

            // ✅ **اینجا** سفارش ایجاد می‌شود + موجودی کاهش می‌یابد
            var createOrderResult = _orderApplication.CreateOrderWithCheckoutInfo(
                User.Identity.Name,
                CheckoutInfo,
                shippingCost
            );

            if (!createOrderResult.IsSuccessed)
            {
                TempData["Error"] = createOrderResult.Message;
                return RedirectToPage();
            }

            // ✅ سفارش ثبت شد — پیام موفقیت و redirect
            TempData["SuccessMessage"] = "سفارش شما با موفقیت ثبت شد!";
            return RedirectToPage("/CurrentOrders");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("/Basket");
        }
    }
}
