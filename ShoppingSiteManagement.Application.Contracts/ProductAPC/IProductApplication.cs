using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.ProductAPC
{
    public interface IProductApplication
    {
        Task<OperationResult> Create(CreateProduct command);
        Task<OperationResult> Edit(EditProduct command);
        OperationResult RemoveDiscount(long id);
        OperationResult ApplyDiscount(long id, double discountPrice);
        OperationResult TogglePopularStatus(long id);
        OperationResult Activate(long id);
        OperationResult UnActivate(long id);
        EditProduct GetDetails(long id);
        List<ProductViewModel> Search(ProductSearchModel searchModel);
        ProductViewModel GetProductBySlug(string slug);
        OperationResult IncreaseStock(long id, int count);
        bool Exists(string name);
    }
}
