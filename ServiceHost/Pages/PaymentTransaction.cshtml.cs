using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.CartAPC;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;

namespace ServiceHost.Pages
{
    [Authorize]
    public class PaymentTransactionModel : PageModel
    {
        private readonly ICartApplication _cartApplication;
        private readonly IOrderApplication _orderApplication;

        public PaymentTransactionModel(ICartApplication cartApplication, IOrderApplication orderApplication)
        {
            _cartApplication = cartApplication;
            _orderApplication = orderApplication;
        }

        [BindProperty]
        public CheckoutDto CheckoutInfo { get; set; }

        public int TotalItemsCount { get; set; }
        public double CartAmount { get; set; }
        public double ShippingCost { get; set; }
        public double FinalTotalAmount { get; set; }

        public IActionResult OnGet()
        {
            // ✅ فقط سبد رو نمایش بده (سفارش را check نکن)
            var cart = _cartApplication.GetCart(User.Identity.Name);

            if (cart == null || !cart.Items.Any())
                return RedirectToPage("/Basket");

            TotalItemsCount = cart.TotalItems;
            CartAmount = cart.TotalAmount;
            ShippingCost = 15000; // ✅ هزینه ارسال ثابت (یا از SiteSettings بگیر)
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

            // ✅ CheckoutInfo.Name رو validate کن
            if (string.IsNullOrWhiteSpace(CheckoutInfo?.Name) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Phone) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Province) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.City) ||
                string.IsNullOrWhiteSpace(CheckoutInfo?.Address))
            {
                TempData["Error"] = "لطفاً تمام فیلدها را پر کنید";
                return RedirectToPage();
            }

            // ✅ **اینجا** درست می‌شود: سفارش ایجاد می‌شود + موجودی کاهش می‌یابد
            var createOrderResult = _orderApplication.CreateOrderWithCheckoutInfo(
                User.Identity.Name,
                CheckoutInfo,
                15000 // شحنه ارسال
            );

            if (!createOrderResult.IsSuccessed)
            {
                TempData["Error"] = createOrderResult.Message;
                return RedirectToPage();
            }

            // ✅ سفارش ثبت شد
            TempData["SuccessMessage"] = "سفارش شما با موفقیت ثبت شد!";
            return RedirectToPage("/CurrentOrders");
        }

        public IActionResult OnPostCancel()
        {
            // ✅ اگر فرم cancel شود، سبد باقی می‌ماند
            return RedirectToPage("/Basket");
        }
    }
}
