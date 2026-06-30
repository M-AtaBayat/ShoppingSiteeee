using _0_Framework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using ShoppingSiteManagement.Infrastructure.EFCore;
using System.Linq;

namespace ServiceHost.Pages
{
    [Authorize]
    public class OrderTrackingModel : PageModel
    {
        private readonly ShoppingSiteContext _context;

        public OrderTrackingModel(ShoppingSiteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TrackingInputModel Command { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string WarningMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "لطفاً اطلاعات را به درستی وارد کنید.";
                return Page();
            }

            var order = _context.Orders.FirstOrDefault(x => x.TrackingCode == Command.TrackingCode);

            if (order == null)
            {
                ErrorMessage = "سفارشی با این کد پیگیری یافت نشد. لطفاً کد را بررسی کنید.";
                return Page();
            }

            if (order.Status == OrderStatus.Delivered)
            {
                WarningMessage = "سفارش شما با این کد پیگیری قبلاً تحویل داده شده است. در صورت بروز مشکل با پشتیبانی تماس بگیرید.";
                return Page();
            }
            
            var message = new OrderContact(Command.PhoneNumber, Command.TrackingCode, Command.Message);
            _context.OrderContacts.Add(message);
            _context.SaveChanges();

            SuccessMessage = "پیگیری شما با موفقیت ثبت شد. همکاران ما در اسرع وقت با شما تماس می‌گیرند.";

            return RedirectToPage();
        }
    }

    public class TrackingInputModel
    {
        public string PhoneNumber { get; set; }
        public string TrackingCode { get; set; }
        public string Message { get; set; }
    }
}
