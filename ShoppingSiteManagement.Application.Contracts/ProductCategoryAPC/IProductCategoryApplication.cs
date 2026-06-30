using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC
{
    public interface IProductCategoryApplication
    {
        OperationResult Create(CreateProductCategory command);
        OperationResult Edit(EditProductCategory command);
        OperationResult Activate(int id);
        OperationResult UnActivate(int id);
        EditProductCategory GetDetails(int id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
        bool ExistsCategory(string name);
    }
}
