using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC
{
    public class CreateProductCategory
    {
        [Required(ErrorMessage = "نام نمی‌تواند خالی باشد.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "دسته‌بندی (DataCategory) نمی‌تواند خالی باشد.")]
        public string Category { get; set; }
    }
}
