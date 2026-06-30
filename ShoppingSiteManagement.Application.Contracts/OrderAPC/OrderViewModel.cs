using _0_Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.OrderAPC
{
    public class OrderViewModel
    {
        public long Id { get; set; }
        public string TrackingCode { get; set; }
        public string PostTrackingCode { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusTitle { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDatePersian { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public DateTime? SentDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
