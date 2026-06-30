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
        public string ErrorMessage { get; set; }


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
            // خلاصه سبد رو هر بار که برگردیم به Page() دوباره می‌سازیم
            // تا صفحه خراب یا با عدد صفر نمایش داده نشه
            void LoadSummary()
            {
                var c = _cartApplication.GetCart(User.Identity.Name);
                var s = _settingsApplication.GetSettings();
                var shipping = s?.ShippingCost ?? 0;

                TotalItemsCount = c?.TotalItems ?? 0;
                CartAmount = c?.TotalAmount ?? 0;
                ShippingCost = shipping;
                FinalTotalAmount = CartAmount + ShippingCost;
            }

            try
            {
                // ✅ Validate کامل
                if (CheckoutInfo == null ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.Name) ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.Phone) ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.Province) ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.City) ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.Postal) ||
                    string.IsNullOrWhiteSpace(CheckoutInfo.Address))
                {
                    ErrorMessage = "لطفاً تمام فیلدها را پر کنید";
                    LoadSummary();
                    return Page();
                }

                var cart = _cartApplication.GetCart(User.Identity.Name);
                if (cart == null || !cart.Items.Any())
                {
                    ErrorMessage = "سبد خرید شما خالی است یا منقضی شده. لطفاً دوباره از سبد خرید اقدام کنید.";
                    LoadSummary();
                    return Page();
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
                    ErrorMessage = createOrderResult.Message;
                    LoadSummary();
                    return Page();
                }

                // ✅ سفارش ثبت شد — پیام موفقیت و redirect
                TempData["SuccessMessage"] = "سفارش شما با موفقیت ثبت شد!";
                return RedirectToPage("/CurrentOrders");
            }
            catch (Exception ex)
            {
                ErrorMessage = "خطایی در ثبت سفارش رخ داد. لطفاً دوباره تلاش کنید.";
                LoadSummary();
                return Page();
            }
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("/Basket");
        }
    }
}
