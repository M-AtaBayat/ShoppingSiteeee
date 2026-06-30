using _0_Framework.Domain;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.ProductAgg
{
    public class Product : EntityBase<long>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string OrginalImage { get; private set; }
        public string MoreImage1 { get; private set; }
        public string MoreImage2 { get; private set; }
        public string Size { get; private set; }
        public string Mark { get; private set; }
        public string Color { get; private set; }
        public double Price { get; private set; }
        public int StockCount { get; private set; }
        public bool HasDiscount { get; private set; }
        public double? DiscountedPrice { get; private set; }
        public bool IsPopular { get; private set; }
        public string Slug { get; private set; }
        public string MetaDescription { get; private set; }
        public string Keywords { get; private set; }

        public int CategoryID { get; private set; }
        public ProductCategory Category { get; private set; }

        protected Product() { }

        public Product(string name, string description, string orginalImage, string moreImage1, string moreImage2,
            string size, string mark, string color, double price, int categoryID, int stockCount, string slug,
            string keywords, string metaDescription)
        {
            Name = name;
            Description = description;
            OrginalImage = orginalImage;
            MoreImage1 = moreImage1;
            MoreImage2 = moreImage2;
            Size = size;
            Mark = mark;
            Color = color;
            Price = price;
            CategoryID = categoryID;
            StockCount = stockCount;
            Slug = slug;
            Keywords = keywords;
            MetaDescription = metaDescription;
            HasDiscount = false;
            IsPopular = false;
        }

        public void Edit(string name, string description, string orginalImage, string moreImage1, string moreImage2,
            string size, string mark, string color, double price, int categoryID, int stockCount, string slug,
            string keywords, string metaDescription)
        {
            Name = name;
            Description = description;
            OrginalImage = orginalImage;
            MoreImage1 = moreImage1;
            MoreImage2 = moreImage2;
            Size = size;
            Mark = mark;
            Color = color;
            Price = price;
            CategoryID = categoryID;
            StockCount = stockCount;
            Slug = slug;
            Keywords = keywords;
            MetaDescription = metaDescription;
        }

        public void ApplyDiscount(double discountedPrice)
        {
            DiscountedPrice = discountedPrice;
            HasDiscount = true;
        }

        public void RemoveDiscount()
        {
            DiscountedPrice = null;
            HasDiscount = false;
        }

        public void TogglePopularStatus()
        {
            IsPopular = !IsPopular;
        }
        public void IncreaseStock(int count)
        {
            StockCount += count;
        }

        public void ReduceStock(int count)
        {
            if (StockCount >= count)
                StockCount -= count;
            else
                throw new Exception("موجودی کافی نیست");
        }
    }
}