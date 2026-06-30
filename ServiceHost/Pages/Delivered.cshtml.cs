using _0_Framework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ServiceHost.Pages
{
    [Authorize]
    public class DeliveredModel : PageModel
    {
        private readonly ShoppingSiteContext _context;

        public List<DeliveredOrderViewModel> Orders { get; set; } = new List<DeliveredOrderViewModel>();

        public DeliveredModel(ShoppingSiteContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var orders = _context.Orders
                .Include(x => x.Items)
                .Where(x => x.AccountEmail == User.Identity.Name && x.Status == OrderStatus.Delivered)
                .OrderByDescending(x => x.Id)
                .ToList();

            foreach (var order in orders)
            {
                var orderVm = new DeliveredOrderViewModel
                {
                    TrackingCode = order.TrackingCode,
                    CreationDate = ToPersianDate(order.CreationDate),
                    DeliveryDate = order.DeliveredDate.HasValue
    ? ToPersianDate(order.DeliveredDate.Value)
    : "در انتظار تحویل",

                    ReceiverName = order.ReceiverName,
                    FinalAmount = order.FinalAmount
                };

                var productSummaries = new List<string>();
                foreach (var item in order.Items)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        productSummaries.Add($"{product.Name} × {item.Count}");
                    }
                }
                orderVm.ProductsSummary = string.Join(" ، ", productSummaries);

                Orders.Add(orderVm);
            }
        }

        private string ToPersianDate(System.DateTime date)
        {
            if (date == System.DateTime.MinValue || date == null) return "---";
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }
    }

    public class DeliveredOrderViewModel
    {
        public string TrackingCode { get; set; }
        public string CreationDate { get; set; }
        public string DeliveryDate { get; set; }
        public string ReceiverName { get; set; }
        public string ProductsSummary { get; set; }
        public double FinalAmount { get; set; }
    }
}
