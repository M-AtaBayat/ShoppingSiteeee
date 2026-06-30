using Microsoft.AspNetCore.Mvc;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageProduct
{
    public class ListModel : AdminPageModel
    {
        [BindProperty(SupportsGet = true)] public ProductSearchModel SearchModel { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public SelectList ProductCategories { get; set; }

        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        public ListModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        public void OnGet()
        {
            var categories = _productCategoryApplication.Search(new ProductCategorySearchModel());
            ProductCategories = new SelectList(categories, "Id", "Name");
            Products = _productApplication.Search(SearchModel);
        }

        public IActionResult OnGetTogglePopular(long id)
        {
            _productApplication.TogglePopularStatus(id);
            return RedirectToPage("./List");
        }

        public IActionResult OnGetRemove(long id)
        {
            _productApplication.UnActivate(id);
            return RedirectToPage("./List");
        }

        public IActionResult OnGetRestore(long id)
        {
            var product = _productApplication.GetDetails(id);

            if (product.StockCount <= 0)
            {
                TempData["ErrorMessage"] = "محصولی که موجودی انبارش صفر است قابل فعال‌سازی نیست!";
                return RedirectToPage("./List");
            }

            _productApplication.Activate(id);
            return RedirectToPage("./List");
        }
        public IActionResult OnPostApplyDiscount(long id, int discountRate)
        {
            var product = _productApplication.GetDetails(id);
            if (product != null)
            {
                double discountedPrice = product.Price - (product.Price * discountRate / 100);
                _productApplication.ApplyDiscount(id, discountedPrice);
            }
            return RedirectToPage("./List");
        }

        public IActionResult OnPostIncreaseStock(long id, int count)
        {
            _productApplication.IncreaseStock(id, count);
            return RedirectToPage("./List");
        }
        
        public IActionResult OnGetRemoveDiscount(long id)
        {
            _productApplication.RemoveDiscount(id);
            return RedirectToPage("./List");
        }

    }
}
