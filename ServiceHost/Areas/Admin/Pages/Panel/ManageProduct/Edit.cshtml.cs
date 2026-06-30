using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProduct
{
    public class EditModel : AdminPageModel
    {
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        [BindProperty]
        public EditProduct Command { get; set; }
        public SelectList Categories { get; set; }

        public EditModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet(long id)
        {
            Command = _productApplication.GetDetails(id);

            var categories = _productCategoryApplication.Search(new ProductCategorySearchModel());
            Categories = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _productApplication.Edit(Command);

            if (result.IsSuccessed)
                return RedirectToPage("./List");

            var categories = _productCategoryApplication.Search(new ProductCategorySearchModel());
            Categories = new SelectList(categories, "Id", "Name");
            ModelState.AddModelError("", result.Message);
            return Page();
        }
    }
}
