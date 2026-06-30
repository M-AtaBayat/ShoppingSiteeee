using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProduct
{
    public class CreateModel : AdminPageModel
    {
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        [BindProperty]
        public CreateProduct Command { get; set; }
        public SelectList Categories { get; set; }

        public CreateModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet()
        {
            var categories = _productCategoryApplication.Search(new ProductCategorySearchModel());
            Categories = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _productApplication.Create(Command);

            if (result.IsSuccessed)
                return RedirectToPage("./List");

            ModelState.AddModelError("", result.Message);
            OnGet();
            return Page();
        }
    }
}
