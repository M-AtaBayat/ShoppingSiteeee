using System.ComponentModel.DataAnnotations;

namespace ShoppingSiteManagement.Application.Contracts.AccountAPC
{
    public class VerifyViewModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
