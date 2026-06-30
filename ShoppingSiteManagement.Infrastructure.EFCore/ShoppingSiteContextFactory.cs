using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Infrastructure.EFCore
{
    public class ShoppingSiteContextFactory : IDesignTimeDbContextFactory<ShoppingSiteContext>
    {
        public ShoppingSiteContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingSiteContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=.;Initial Catalog=ParsoShopDb;User Id=sa; Password=2522513381388;TrustServerCertificate=True;"
                //"Data Source=45.92.94.8;Initial Catalog=ParsoDB;User Id=pars-gAi94_parsoAdminshop; Password=$pZqWXuim593n#pq;TrustServerCertificate=True;"
            );
            return new ShoppingSiteContext(optionsBuilder.Options);
        }
    }
}
