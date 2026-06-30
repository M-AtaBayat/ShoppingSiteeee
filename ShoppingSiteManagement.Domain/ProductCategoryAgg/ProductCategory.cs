using _0_Framework.Domain;
using ShoppingSiteManagement.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.ProductCategoryAgg
{
    public class ProductCategory : EntityBase<int>
    {
        public string Name { get; private set; }
        public string Category { get; private set; }
        public List<Product> Products { get; private set; }

        protected ProductCategory() { }

        public ProductCategory(string name, string category)
        {
            Name = name;
            Category = category;
            Products = new List<Product>();
        }

        public void Edit(string name, string category)
        {
            Name = name;
            Category = category;
        }
    }
}
