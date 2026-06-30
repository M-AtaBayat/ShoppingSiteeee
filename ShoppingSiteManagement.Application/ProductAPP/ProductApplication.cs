
using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.ProductAPP
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly FileUploader _fileUploader;

        public ProductApplication(IProductRepository productRepository, FileUploader fileUploader)
        {
            _productRepository = productRepository;
            _fileUploader = fileUploader;
        }

        public async Task<OperationResult> Create(CreateProduct command)
        {
            var operation = new OperationResult();
            if (_productRepository.Exists(x => x.Name == command.Name))
                return operation.Failed("امکان ثبت محصول تکراری وجود ندارد.");

            var orginalImagePath = await _fileUploader.UploadAsync(command.OrginalImageFile, "Products");
            var moreImage1Path = command.MoreImage1File != null ? await _fileUploader.UploadAsync(command.MoreImage1File, "Products") : command.MoreImage1;
            var moreImage2Path = command.MoreImage2File != null ? await _fileUploader.UploadAsync(command.MoreImage2File, "Products") : command.MoreImage2;

            var product = new Product(command.Name, command.Description, orginalImagePath, moreImage1Path, moreImage2Path,
                command.Size, command.Mark, command.Color, command.Price, command.CategoryID, command.StockCount, command.Slug,command.Keywords,command.MetaDescription);

            _productRepository.Add(product);
            return operation.Success("محصول با موفقیت ثبت شد.");
        }

        public async Task<OperationResult> Edit(EditProduct command)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(command.Id);
            if (product == null)
                return operation.Failed("محصول یافت نشد.");

            double oldPrice = product.Price;
            bool hadDiscount = product.HasDiscount;
            double? oldDiscountedPrice = product.DiscountedPrice;

            var originalImagePath = product.OrginalImage;
            if (command.OrginalImageFile != null)
                originalImagePath = await _fileUploader.UploadAsync(command.OrginalImageFile, "ProductPictures");

            var moreImage1Path = product.MoreImage1;
            if (command.MoreImage1File != null)
                moreImage1Path = await _fileUploader.UploadAsync(command.MoreImage1File, "ProductPictures");

            var moreImage2Path = product.MoreImage2;
            if (command.MoreImage2File != null)
                moreImage2Path = await _fileUploader.UploadAsync(command.MoreImage2File, "ProductPictures");

            product.Edit(command.Name, command.Description, originalImagePath, moreImage1Path, moreImage2Path,
                command.Size, command.Mark, command.Color, command.Price, command.CategoryID, product.StockCount, command.Slug,
                command.Keywords, command.MetaDescription);

            if (hadDiscount && oldPrice != command.Price && oldDiscountedPrice.HasValue && oldPrice > 0)
            {
                double discountRate = (oldPrice - oldDiscountedPrice.Value) / oldPrice;

                double newDiscountedPrice = command.Price - (command.Price * discountRate);

                product.ApplyDiscount(newDiscountedPrice);
            }

            _productRepository.Save();
            return operation.Success();
        }



        public OperationResult RemoveDiscount(long id)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product == null)
                return operation.Failed("محصول یافت نشد.");

            product.RemoveDiscount();
            _productRepository.Save();
            return operation.Success();
        }
        public OperationResult IncreaseStock(long id, int count)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product == null)
                return operation.Failed("محصول مورد نظر یافت نشد.");

            var newStock = product.StockCount + count;

            if (newStock < 0)
                return operation.Failed("موجودی نمی‌تواند منفی شود.");

            product.IncreaseStock(count);

            if (product.StockCount == 0)
            {
                product.UnActivate();
            }
            else if (product.StockCount > 0 && product.IsDeleted)
            {
                product.Activate();
            }

            _productRepository.Save();
            return operation.Success("موجودی و وضعیت محصول بروزرسانی شد.");
        }


        public OperationResult ApplyDiscount(long id, double discountPrice)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product == null)
                return operation.Failed("محصول یافت نشد.");

            product.ApplyDiscount(discountPrice);
            _productRepository.Save();
            return operation.Success();
        }

        public OperationResult TogglePopularStatus(long id)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product == null)
                return operation.Failed("محصول یافت نشد.");

            product.TogglePopularStatus();
            _productRepository.Save();
            return operation.Success();
        }

        public OperationResult Activate(long id)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return operation.Failed("داده مورد نظر یافت نشد.");
            }

            product.Activate();
            _productRepository.Save();
            return operation.Success();
        }

        public OperationResult UnActivate(long id)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(id);
            if (product==null)
            {
                return operation.Failed("داده مورد نظر یافت نشد.");
            }

            product.UnActivate();
            _productRepository.Save();
            return operation.Success();
        }

        public EditProduct GetDetails(long id)
        {
            return _productRepository.GetDetails(id);
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return _productRepository.Search(searchModel);
        }

        public ProductViewModel GetProductBySlug(string slug)
        {
            var product = _productRepository.GetProductBySlug(slug);

            if (product == null)
                return null;

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                OrginalImage = product.OrginalImage,
                MoreImage1 = product.MoreImage1,
                MoreImage2 = product.MoreImage2,
                Size = product.Size,
                Mark = product.Mark,
                Color = product.Color,
                Price = product.Price,
                StockCount = product.StockCount,
                HasDiscount = product.HasDiscount,
                DiscountedPrice = product.DiscountedPrice,
                IsPopular = product.IsPopular,
                Slug = product.Slug,
                IsDeleted = product.IsDeleted,
                CreationDate = product.CreationDate.ToString(),
                Category = product.Category?.Name,
                CategoryID = product.CategoryID
            };
        }

        public bool Exists(string name)
        {
            return _productRepository.Exists(x => x.Name == name);
        }
    }
}
