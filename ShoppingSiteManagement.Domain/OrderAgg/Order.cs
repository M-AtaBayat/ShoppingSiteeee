using _0_Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.OrderAgg
{
    public class Order : EntityBase<long>
    {
        public string TrackingCode { get; private set; }
        public string PostTrackingCode { get; private set; }
        public OrderStatus Status { get; private set; }
        public string ReceiverName { get; private set; }
        public string ReceiverPhoneNumber { get; private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string Address { get; private set; }
        public double TotalProductsPrice { get; private set; }
        public double ShippingCost { get; private set; }
        public double FinalAmount { get; private set; }
        public DateTime? SentDate { get; private set; }
        public DateTime? DeliveredDate { get; private set; }
        public string AccountEmail { get; private set; }

        public List<OrderItem> Items { get; private set; }

        protected Order() { }

        public Order(string accountEmail, string trackingCode, double totalProductsPrice, double shippingCost,
            string receiverName, string receiverPhoneNumber, string province, string city, string postalCode, string address)
        {
            AccountEmail = accountEmail;
            TrackingCode = trackingCode;
            TotalProductsPrice = totalProductsPrice;
            ShippingCost = shippingCost;
            FinalAmount = totalProductsPrice + shippingCost;
            ReceiverName = receiverName;
            ReceiverPhoneNumber = receiverPhoneNumber;
            Province = province;
            City = city;
            PostalCode = postalCode;
            Address = address;
            Status = OrderStatus.Processing;
            Items = new List<OrderItem>();
        }

        public void SetAsSent(string postTrackingCode)
        {
            Status = OrderStatus.Sent;
            SentDate = DateTime.Now;
            PostTrackingCode = postTrackingCode;
        }

        public void SetAsDelivered()
        {
            Status = OrderStatus.Delivered;
            DeliveredDate = DateTime.Now;
        }

        
        public void EditCheckoutInfo(string receiverName, string receiverPhoneNumber,
            string province, string city, string postalCode, string address)
        {
            ReceiverName = receiverName;
            ReceiverPhoneNumber = receiverPhoneNumber;
            Province = province;
            City = city;
            PostalCode = postalCode;
            Address = address;
            FinalAmount = TotalProductsPrice + ShippingCost;
        }
    }
}