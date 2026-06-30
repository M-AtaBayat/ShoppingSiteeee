using System.ComponentModel.DataAnnotations;

namespace ShoppingSiteManagement.Application.Contracts.OrderContactAPC
{
    public class RegisterOrderContact
        {
            [Required(ErrorMessage = "شماره تماس الزامی است")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "کد پیگیری سفارش الزامی است")]
            public string TrackingCode { get; set; }

            [Required(ErrorMessage = "متن پیام الزامی است")]
            public string Message { get; set; }
    }
}