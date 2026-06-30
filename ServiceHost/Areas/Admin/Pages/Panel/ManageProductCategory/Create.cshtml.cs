using Microsoft.AspNetCore.Mvc;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProductCategory
{
    public class CreateModel : AdminPageModel
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        [BindProperty]
        public CreateProductCategory Command { get; set; }

        public CreateModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = _productCategoryApplication.Create(Command);

            if (result.IsSuccessed)
            {
                return RedirectToPage("./List");
            }

            ModelState.AddModelError("", result.Message);
            return Page();
        }
    }
}
