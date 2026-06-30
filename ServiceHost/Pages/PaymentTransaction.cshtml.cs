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
        public CheckoutDto CheckoutInfo { get; set; } // استفاده از دی‌تی‌او جدید لایه اپلیکیشن

        public int TotalItemsCount { get; set; }
        public double CartAmount { get; set; }
        public double ShippingCost { get; set; }
        public double FinalTotalAmount { get; set; }

        public IActionResult OnGet()
        {
            var order = _orderApplication.GetActiveOrderForCheckout(User.Identity.Name);

            // 🛑 لایف‌سیور: اگر سفارش ثبت نشده باشد، اینجا نال برمی‌گردد و ردیراکت می‌شود به سبد خرید.
            if (order == null)
                return RedirectToPage("/Basket");

            TotalItemsCount = order.TotalItemsCount;
            CartAmount = order.TotalProductsPrice;
            ShippingCost = order.ShippingCost;
            FinalTotalAmount = order.FinalAmount;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            var isSuccess = _orderApplication.FinalizeCheckoutInfo(User.Identity.Name, CheckoutInfo);

            if (isSuccess)
            {
                return RedirectToPage("/CurrentOrders");
            }

            return RedirectToPage("/Basket");
        }

        public IActionResult OnPostCancel()
        {
            _cartApplication.ReleaseReservedStock(User.Identity.Name);
            return RedirectToPage("/Basket");
        }

        public IActionResult OnGetTimeout()
        {
            _cartApplication.ReleaseReservedStock(User.Identity.Name);
            TempData["StockErrorMessage"] = "زمان ۱۰ دقیقه شما به پایان رسید و موجودی به انبار برگشت. لطفا دوباره تلاش کنید.";
            return RedirectToPage("/Basket");
        }
    }
}