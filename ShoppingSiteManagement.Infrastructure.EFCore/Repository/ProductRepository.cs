using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long, Product>, IProductRepository
    {
        private readonly ShoppingSiteContext _context;
        public ProductRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public EditProduct GetDetails(long id)
        {
            return _context.Products
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .Select(x => new EditProduct
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryID = x.CategoryID,
                    Color = x.Color,
                    Description = x.Description,
                    Mark = x.Mark,
                    MoreImage1 = x.MoreImage1,
                    MoreImage2 = x.MoreImage2,
                    OrginalImage = x.OrginalImage,
                    Price = x.Price,
                    Size = x.Size,
                    MetaDescription = x.MetaDescription,
                    Keywords = x.Keywords,Slug = x.Slug,StockCount = x.StockCount,
                }).FirstOrDefault()!;
        }
        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            var persianCalendar = new PersianCalendar();
            var query = _context.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Name = x.Name,
                Description = x.Description,
                OrginalImage = x.OrginalImage,
                MoreImage1 = x.MoreImage1,
                MoreImage2 = x.MoreImage2,
                Size = x.Size,
                Mark = x.Mark,
                Color = x.Color,
                Price = x.Price,
                StockCount = x.StockCount,
                HasDiscount = x.HasDiscount,
                DiscountedPrice = x.DiscountedPrice,
                IsPopular = x.IsPopular,
                Slug = x.Slug,
                IsDeleted = x.IsDeleted,
                CreationDate = persianCalendar.GetYear(x.CreationDate) + "/" +
                    persianCalendar.GetMonth(x.CreationDate).ToString("00") + "/" +
                    persianCalendar.GetDayOfMonth(x.CreationDate).ToString("00") + " " +
                    x.CreationDate.Hour.ToString("00") + ":" +
                    x.CreationDate.Minute.ToString("00") + ":" +
                    x.CreationDate.Second.ToString("00"),
                Category = x.Category.Name,
                CategoryID = x.CategoryID,
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (searchModel.CategoryID > 0)
                query = query.Where(x => x.CategoryID == searchModel.CategoryID);

            if (searchModel.IsPopular)
                query = query.Where(x => x.IsPopular);

            if (searchModel.HasDiscount)
                query = query.Where(x => x.HasDiscount);

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public Product GetProductBySlug(string slug)
        {
            return _context.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Slug == slug);
        }
    }
}