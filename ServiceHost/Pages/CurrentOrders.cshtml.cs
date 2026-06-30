using _0_Framework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CurrentOrdersModel : PageModel
    {
        private readonly ShoppingSiteContext _context;

        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

        public CurrentOrdersModel(ShoppingSiteContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var orders = _context.Orders
                .Include(x => x.Items)
                .Where(x => x.AccountEmail == User.Identity.Name && x.Status != OrderStatus.Delivered)
                .OrderByDescending(x => x.Id)
                .ToList();

            foreach (var order in orders)
            {
                var orderVm = new OrderViewModel
                {
                    TrackingCode = order.TrackingCode,
                    Status = GetStatusName(order.Status),
                    StatusCssClass = GetStatusCssClass(order.Status),
                    CreationDate = ToPersianDate(order.CreationDate),
                    ReceiverPhone = order.ReceiverPhoneNumber,
                    Address = $"{order.Province}، {order.City}، {order.Address}",
                    FinalAmount = order.FinalAmount,
                    Items = new List<OrderItemViewModel>()
                };

                foreach (var item in order.Items)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        orderVm.Items.Add(new OrderItemViewModel
                        {
                            ProductName = product.Name,
                            Size = product.Size ?? "-",
                            Color = product.Color ?? "#000",
                            Price = item.UnitPrice
                        });
                    }
                }

                Orders.Add(orderVm);
            }
        }


        private string GetStatusName(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Processing: return "در حال پردازش";
                case OrderStatus.Sent: return "ارسال شده";
                default: return "نامشخص";
            }
        }

        private string GetStatusCssClass(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Processing: return "processing";
                case OrderStatus.Sent: return "shipped";
                default: return "";
            }
        }

        private string ToPersianDate(DateTime date)
        {
            if (date == default) return "نامشخص";
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00} - {date:HH:mm}";
        }
    }

    public class OrderViewModel
    {
        public string TrackingCode { get; set; }
        public string Status { get; set; }
        public string StatusCssClass { get; set; }
        public string CreationDate { get; set; }
        public string ReceiverPhone { get; set; }
        public string Address { get; set; }
        public double FinalAmount { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
    }
}
