using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using System.Collections.Generic;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProductCategory
{
    public class ListModel : AdminPageModel
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        public List<ProductCategoryViewModel> ProductCategories { get; set; }

        [BindProperty(SupportsGet = true)]
        public ProductCategorySearchModel SearchModel { get; set; }

        public ListModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet(ProductCategorySearchModel searchModel)
        {
            ProductCategories = _productCategoryApplication.Search(searchModel);
        }

        public IActionResult OnGetActivate(int id)
        {
            _productCategoryApplication.Activate(id);
            return RedirectToPage("./List");
        }

        public IActionResult OnGetUnActivate(int id)
        {
            _productCategoryApplication.UnActivate(id);
            return RedirectToPage("./List");
        }
    }
}
