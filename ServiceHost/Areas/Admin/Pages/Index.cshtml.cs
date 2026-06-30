using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceHost.Areas.Admin.Pages
{
    public class IndexModel : AdminPageModel
    {
        private readonly IOrderApplication _orderApplication;

        public int SalesCount { get; set; }
        public double RevenueCount { get; set; }
        public int CustomersCount { get; set; }
        public List<OrderViewModel> RecentOrders { get; set; }

        public IndexModel(IOrderApplication orderApplication)
        {
            _orderApplication = orderApplication;
        }

        public void OnGet()
        {
            var orders = _orderApplication.Search(new SearchOrderModel());

            if (orders != null && orders.Any())
            {
                SalesCount = orders.Count;
                RevenueCount = orders.Sum(x => x.FinalAmount);
                CustomersCount = orders.Select(x => x.ReceiverPhoneNumber).Distinct().Count();
                RecentOrders = orders.OrderByDescending(x => x.Id).Take(20).ToList();
            }
            else
            {
                SalesCount = 0;
                RevenueCount = 0;
                CustomersCount = 0;
                RecentOrders = new List<OrderViewModel>();
            }
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login", new { area = "Admin" });
        }
    }
}
