using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.OrderCheckoutAPC
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "نام گیرنده الزامی است.")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "شماره تماس الزامی است.")]
        public string ReceiverPhoneNumber { get; set; }

        [Required(ErrorMessage = "استان الزامی است.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "شهرستان الزامی است.")]
        public string City { get; set; }

        [Required(ErrorMessage = "کد پستی الزامی است.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "آدرس الزامی است.")]
        public string Address { get; set; }

        public double TotalProductsPrice { get; set; }
        public double ShippingCost { get; set; }
        public double FinalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
