using System.Collections.Generic;

namespace _01_ShoppingSiteQuery.Contracts.Product
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestProducts();
        List<ProductQueryModel> GetProductsWithDiscount();
        List<ProductQueryModel> GetPopularProducts();
        ProductQueryModel GetProductDetails(string slug);
    }
}