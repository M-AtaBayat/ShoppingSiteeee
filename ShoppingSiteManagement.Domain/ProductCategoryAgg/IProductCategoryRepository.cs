using _0_Framework.Domain;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IRepository<int, ProductCategory>
    {
        EditProductCategory GetDetails(int id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
    }
}
