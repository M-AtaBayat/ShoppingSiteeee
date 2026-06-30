using Microsoft.AspNetCore.Mvc;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProductCategory
{
    public class EditModel : AdminPageModel
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        [BindProperty]
        public EditProductCategory Command { get; set; }

        public EditModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet(int id)
        {
            Command = _productCategoryApplication.GetDetails(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _productCategoryApplication.Edit(Command);

            if (result.IsSuccessed)
                return RedirectToPage("./List");

            ModelState.AddModelError("", result.Message);
            return Page();
        }
    }
}
