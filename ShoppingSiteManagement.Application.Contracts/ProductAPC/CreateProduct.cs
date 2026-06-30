using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.ProductAPC
{
    public class CreateProduct
    {
        [Required(ErrorMessage = "نام نمی‌تواند خالی باشد.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "توضیحات نمی‌تواند خالی باشد.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "تصویر اصلی نمی‌تواند خالی باشد.")]
        public string OrginalImage { get; set; }
        public IFormFile OrginalImageFile { get; set; }

        public string MoreImage1 { get; set; }
        public IFormFile MoreImage1File { get; set; }

        public string MoreImage2 { get; set; }
        public IFormFile MoreImage2File { get; set; }

        [Required(ErrorMessage = "سایز نمی‌تواند خالی باشد.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "مارک نمی‌تواند خالی باشد.")]
        public string Mark { get; set; }

        [Required(ErrorMessage = "رنگ نمی‌تواند خالی باشد.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "قیمت نمی‌تواند خالی باشد.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "دسته‌بندی نمی‌تواند خالی باشد.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "موجودی نمی‌تواند خالی باشد.")]
        public int StockCount { get; set; }

        [Required(ErrorMessage = "Slug نمی‌تواند خالی باشد.")]
        public string Slug { get; set; }
        [Required(ErrorMessage = "Keywords نمی‌تواند خالی باشد.")]
        public string Keywords { get; set; }
        [Required(ErrorMessage = "MetaDescription نمی‌تواند خالی باشد.")]
        public string MetaDescription { get; set; }
    }
}
