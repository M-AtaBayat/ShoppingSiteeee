using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.SettingsAPC
{
    public class EditSettings
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "هزینه ارسال الزامی است.")]
        public double ShippingCost { get; set; }

        [Required(ErrorMessage = "ایمیل ادمین الزامی است.")]
        public string AdminEmail { get; set; }
    }
}
