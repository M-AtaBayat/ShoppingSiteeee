using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.ProductCategoryAPP
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProductCategory command)
        {   
            var operation = new OperationResult();
            if (_productCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed("امکان ثبت دسته‌بندی تکراری وجود ندارد.");

            var category = new ProductCategory(command.Name, command.Category);
            _productCategoryRepository.Add(category);
            return operation.Success("دسته‌بندی با موفقیت ثبت شد.");
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var category = _productCategoryRepository.Get(command.Id);
            if (category == null)
                return operation.Failed("داده مورد نظر یافت نشد.");

            if (_productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed("امکان ثبت دسته‌بندی تکراری وجود ندارد.");

            category.Edit(command.Name, command.Category);
            _productCategoryRepository.Save();
            return operation.Success("ویرایش دسته‌بندی با موفقیت انجام شد.");
        }

        public OperationResult Activate(int id)
        {
            var operation = new OperationResult();
            var category = _productCategoryRepository.Get(id);
            if (category == null)
                return operation.Failed("داده مورد نظر یافت نشد.");

            category.Activate();
            _productCategoryRepository.Save();
            return operation.Success();
        }

        public OperationResult UnActivate(int id)
        {
            var operation = new OperationResult();
            var category = _productCategoryRepository.Get(id);
            if (category == null)
                return operation.Failed("داده مورد نظر یافت نشد.");

            category.UnActivate();
            _productCategoryRepository.Save();
            return operation.Success();
        }

        public EditProductCategory GetDetails(int id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }

        public bool ExistsCategory(string name)
        {
            return _productCategoryRepository.Exists(x => x.Name == name);
        }
    }
}