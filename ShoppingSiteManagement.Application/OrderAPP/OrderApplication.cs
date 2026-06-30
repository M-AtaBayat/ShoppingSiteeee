using _0_Framework.Application;
using _0_Framework.Domain;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using ShoppingSiteManagement.Domain.OrderAgg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShoppingSiteManagement.Application.OrderAPP
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderRepository _orderRepository;

        public OrderApplication(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // 🟢 متد جدید برای ایجاد سفارش از سبد خریدی
        public OperationResult CreateOrderFromCart(CreateOrderFromCartDto createOrderDto)
        {
            var operation = new OperationResult();

            try
            {
                if (createOrderDto == null || !createOrderDto.Items.Any())
                    return operation.Failed("سبد خریدی خالی است.");

                // تولید کد ردیابی یکتا
                var trackingCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);

                // ایجاد سفارش جدید
                var order = new Order(
                    accountEmail: createOrderDto.AccountEmail,
                    trackingCode: trackingCode,
                    totalProductsPrice: createOrderDto.TotalProductsPrice,
                    shippingCost: createOrderDto.ShippingCost,
                    receiverName: "", // اطلاعات بعد‌تر در صفحه نهایی‌سازی تکمیل می‌شود
                    receiverPhoneNumber: "",
                    province: "",
                    city: "",
                    postalCode: "",
                    address: ""
                );

                // اضافه کردن آیتم‌های سفارش
                foreach (var item in createOrderDto.Items)
                {
                    var orderItem = new OrderItem(
                        orderId: 0, // Order ID هنوز set نشده (به‌طور خودکار توسط EF تنظیم می‌شود)
                        productId: item.ProductId,
                        productName: item.ProductName,
                        productImage: item.ProductImage,
                        unitPrice: item.UnitPrice,
                        count: item.Count
                    );

                    order.Items.Add(orderItem);
                }

                // ذخیره سفارش — این فقط به DbContext اضافه می‌کند، اما SAVE نمی‌کند
                // SaveChanges در Checkout انجام می‌شود (که درون تراکنش است)
                _orderRepository.Add(order);
                // حذف این خط: _orderRepository.Save();

                return operation.Success($"سفارش با کد ردیابی {trackingCode} ایجاد شد.");
            }
            catch (Exception ex)
            {
                return operation.Failed($"خطا در ایجاد سفارش: {ex.Message}");
            }
        }
        public OperationResult ConfirmOrder(long id, string postTrackingCode)
        {
            var operation = new OperationResult();
            var order = _orderRepository.GetDetails(id);
            if (order == null)
                return operation.Failed("سفارش یافت نشد.");

            order.SetAsSent(postTrackingCode);
            _orderRepository.Save();
            return operation.Success("سفارش به وضعیت ارسال شده تغییر یافت.");
        }

        public OperationResult DeliverOrder(long id)
        {
            var operation = new OperationResult();
            var order = _orderRepository.GetDetails(id);
            if (order == null)
                return operation.Failed("سفارش یافت نشد.");

            order.SetAsDelivered();
            _orderRepository.Save();
            return operation.Success("سفارش به وضعیت تحویل شده تغییر یافت.");
        }

        public OrderViewModel GetDetails(long id)
        {
            var order = _orderRepository.GetDetails(id);
            return order == null ? null : MapToViewModel(order);
        }

        public List<OrderViewModel> Search(SearchOrderModel searchModel)
        {
            var orders = _orderRepository.GetAll();
            if (orders == null) return new List<OrderViewModel>();

            var query = orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.TrackingCode))
                query = query.Where(x => x.TrackingCode.Contains(searchModel.TrackingCode));

            if (!string.IsNullOrWhiteSpace(searchModel.PostTrackingCode))
                query = query.Where(x => x.PostTrackingCode.Contains(searchModel.PostTrackingCode));

            if (searchModel.Status > 0)
                query = query.Where(x => x.Status == (OrderStatus)searchModel.Status);

            if (!string.IsNullOrWhiteSpace(searchModel.ReceiverName))
                query = query.Where(x => x.ReceiverName.Contains(searchModel.ReceiverName));

            return query.Select(x => MapToViewModel(x))
                       .OrderByDescending(x => x.Id)
                       .ToList();
        }

        public List<OrderViewModel> GetOrdersByAccountEmail(string email)
        {
            var orders = _orderRepository.GetOrdersByAccountEmail(email);
            if (orders == null) return new List<OrderViewModel>();

            return orders.Select(x => MapToViewModel(x))
                       .OrderByDescending(x => x.Id)
                       .ToList();
        }

        public OrderViewModel GetByTrackingCode(string trackingCode)
        {
            var order = _orderRepository.GetByTrackingCode(trackingCode);
            return order == null ? null : MapToViewModel(order);
        }

        public OrderViewModel GetByPostTrackingCode(string postTrackingCode)
        {
            var order = _orderRepository.GetByPostTrackingCode(postTrackingCode);
            return order == null ? null : MapToViewModel(order);
        }

        private OrderViewModel MapToViewModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                TrackingCode = order.TrackingCode,
                PostTrackingCode = order.PostTrackingCode,
                Status = order.Status,
                StatusTitle = GetStatusTitle(order.Status),
                FinalAmount = order.FinalAmount,
                CreationDate = order.CreationDate,
                CreationDatePersian = ToPersianDate(order.CreationDate),
                ReceiverName = order.ReceiverName,
                ReceiverPhoneNumber = order.ReceiverPhoneNumber,
                Province = order.Province,
                City = order.City,
                Address = order.Address,
                PostalCode = order.PostalCode,
                SentDate = order.SentDate,
                DeliveredDate = order.DeliveredDate,

                Items = order.Items?.Select(x => new OrderItemViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    UnitPrice = x.UnitPrice,
                    Count = x.Count,
                    TotalItemPrice = (x.UnitPrice * x.Count)
                }).ToList()
            };
        }

        private string GetStatusTitle(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Processing: return "در حال پردازش";
                case OrderStatus.Sent: return "ارسال شده";
                case OrderStatus.Delivered: return "تحویل شده";
                default: return "";
            }
        }

        private string ToPersianDate(DateTime date)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");
        }

        public ActiveOrderDto GetActiveOrderForCheckout(string accountEmail)
        {
            // پیدا کردن سفارشی که هنوز ارسال یا تحویل نشده و در مرحله پرداخت است
            var order = _orderRepository.GetOrdersByAccountEmail(accountEmail)
                .Where(x => x.Status != OrderStatus.Sent && x.Status != OrderStatus.Delivered)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (order == null) return null;

            return new ActiveOrderDto
            {
                TotalItemsCount = order.Items.Sum(x => x.Count),
                TotalProductsPrice = order.TotalProductsPrice,
                ShippingCost = order.ShippingCost,
                FinalAmount = order.FinalAmount
            };
        }

        public bool FinalizeCheckoutInfo(string accountEmail, CheckoutDto checkoutInfo)
        {
            var order = _orderRepository.GetOrdersByAccountEmail(accountEmail)
                .Where(x => x.Status != OrderStatus.Sent && x.Status != OrderStatus.Delivered)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (order == null) return false;

            order.EditCheckoutInfo(
                checkoutInfo.Name,
                checkoutInfo.Phone,
                checkoutInfo.Province,
                checkoutInfo.City,
                checkoutInfo.Postal,
                checkoutInfo.Address
            );

            _orderRepository.Save();
            return true;
        }
    }
}