using _01_ShoppingSiteQuery.Contracts.Product;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShoppingSiteQuery.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShoppingSiteContext _context;

        public ProductQuery(ShoppingSiteContext context)
        {
            _context = context;
        }

        public List<ProductQueryModel> GetLatestProducts()
        {
            return _context.Products
                .Where(x => x.IsDeleted == false && x.StockCount > 0)
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    Size = x.Size,
                    StockCount = x.StockCount,
                    Picture = x.OrginalImage,
                    Price = x.Price,
                    DiscountedPrice = x.DiscountedPrice ?? 0,
                    HasDiscount = x.HasDiscount,
                    IsPopular = x.IsPopular,
                    Slug = x.Slug,
                    Category = x.Category.Name
                })
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x => CalculateData(x)).ToList();
        }

        public List<ProductQueryModel> GetProductsWithDiscount()
        {
            return _context.Products
                .Include(x => x.Category)
                .Where(x => x.HasDiscount)
                .Where(x => x.IsDeleted == false && x.StockCount > 0)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    Size = x.Size,
                    Picture = x.OrginalImage,
                    Price = x.Price,
                    DiscountedPrice = x.DiscountedPrice ?? 0,
                    HasDiscount = true,
                    StockCount = x.StockCount,
                    IsPopular = x.IsPopular,
                    Slug = x.Slug,
                    Category = x.Category.Name
                })
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x => CalculateData(x)).ToList();
        }

        public List<ProductQueryModel> GetPopularProducts()
        {

            return _context.Products
                .Include(x => x.Category)
                .Where(x => x.IsPopular)
                .Where(x => x.IsDeleted == false && x.StockCount > 0)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.OrginalImage,
                    Price = x.Price,
                    Color = x.Color,
                    Size = x.Size,
                    DiscountedPrice = x.DiscountedPrice ?? 0,
                    HasDiscount = x.HasDiscount,
                    IsPopular = true,
                    Slug = x.Slug,
                    Category = x.Category.Name
                })
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x => CalculateData(x)).ToList();
        }

        private static ProductQueryModel CalculateData(ProductQueryModel product)
        {
            if (product.HasDiscount && product.DiscountedPrice > 0)
            {
                product.PriceWithDiscount = product.DiscountedPrice.ToString("N0");
                
                if (product.Price > 0)
                {
                    double discount = product.Price - product.DiscountedPrice;
                    product.DiscountRate = (int)((discount * 100) / product.Price);
                }
            }
            else
            {
                product.PriceWithDiscount = product.Price.ToString("N0");
            }

            return product;
        }

        public ProductQueryModel GetProductDetails(string slug)
        {
            var product = _context.Products
                .Include(x => x.Category)
                .Where(x => x.IsDeleted == false && x.StockCount > 0)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Picture = x.OrginalImage,
                    StockCount = x.StockCount,
                    Picture1 = x.MoreImage1,
                    Picture2 = x.MoreImage2,
                    Price = x.Price,
                    DiscountedPrice = x.DiscountedPrice ?? 0,
                    HasDiscount = x.HasDiscount,
                    Slug = x.Slug,
                    Color = x.Color,
                    Size = x.Size,
                    Mark = x.Mark,
                    Category = x.Category.Name,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription
                })
                .FirstOrDefault(x => x.Slug == slug);

            if (product == null) return null;

            return CalculateData(product);
        }
    }
}