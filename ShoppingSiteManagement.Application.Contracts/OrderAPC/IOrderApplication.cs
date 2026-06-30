using _0_Framework.Application;
using System.Collections.Generic;

namespace ShoppingSiteManagement.Application.Contracts.OrderAPC
{
    // مدل ورودی فرم نهایی‌سازی که به این لایه منتقل شد
    public class CheckoutDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Address { get; set; }
    }

    public class ActiveOrderDto
    {
        public int TotalItemsCount { get; set; }
        public double TotalProductsPrice { get; set; }
        public double ShippingCost { get; set; }
        public double FinalAmount { get; set; }
    }

    // DTO برای ایجاد سفارش از سبد خرید
    public class CreateOrderFromCartDto
    {
        public string AccountEmail { get; set; }
        public double TotalProductsPrice { get; set; }
        public double ShippingCost { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }

    public class CreateOrderItemDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double UnitPrice { get; set; }
        public int Count { get; set; }
    }

    public interface IOrderApplication
    {
        // 🟢 متد جدید برای ایجاد سفارش از سبد خریدی
        OperationResult CreateOrderFromCart(CreateOrderFromCartDto createOrderDto);

        // 🟢 متد برای ایجاد سفارش با اطلاعات نهایی
        OperationResult CreateOrderWithCheckoutInfo(string accountEmail, CheckoutDto checkoutInfo, double shippingCost);

        OperationResult ConfirmOrder(long id, string postTrackingCode);
        OperationResult DeliverOrder(long id);
        OrderViewModel GetDetails(long id);
        List<OrderViewModel> Search(SearchOrderModel searchModel);
        List<OrderViewModel> GetOrdersByAccountEmail(string email);
        OrderViewModel GetByTrackingCode(string trackingCode);
        OrderViewModel GetByPostTrackingCode(string postTrackingCode);
        ActiveOrderDto GetActiveOrderForCheckout(string accountEmail);
        bool FinalizeCheckoutInfo(string accountEmail, CheckoutDto checkoutInfo);
    }
}
