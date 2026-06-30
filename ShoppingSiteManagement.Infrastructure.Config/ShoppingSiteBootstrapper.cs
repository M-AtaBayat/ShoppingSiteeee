using _0_Framework.Application;
using _01_ShoppingSiteQuery.Contracts.ProductCategory;
using _01_ShoppingSiteQuery.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingSiteManagement.Application.AccountAPP;
using ShoppingSiteManagement.Application.CartAPP;
using ShoppingSiteManagement.Application.Contracts.AccountAPC;
using ShoppingSiteManagement.Application.Contracts.CartAPC;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;
using ShoppingSiteManagement.Application.Contracts.ProductAPC;
using ShoppingSiteManagement.Application.Contracts.ProductCategoryAPC;
using ShoppingSiteManagement.Application.Contracts.SettingsAPC;
using ShoppingSiteManagement.Application.OrderAPP;
using ShoppingSiteManagement.Application.OrderContactAPP;
using ShoppingSiteManagement.Application.ProductAPP;
using ShoppingSiteManagement.Application.ProductCategoryAPP;
using ShoppingSiteManagement.Application.SettingsAPP;
using ShoppingSiteManagement.Domain.AccountAgg;
using ShoppingSiteManagement.Domain.CartAgg;
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using ShoppingSiteManagement.Domain.ProductAgg;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;
using ShoppingSiteManagement.Domain.SettingsAgg;
using ShoppingSiteManagement.Infrastructure.EFCore;
using ShoppingSiteManagement.Infrastructure.EFCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Infrastructure.Config
{
    public class ShoppingSiteBootstrapper
    {
        public static void ShoppingSiteConfig(IServiceCollection services, string con)
        {
            // Product
            services.AddTransient<IProductApplication, ProductApplication>();
            services.AddTransient<IProductRepository, ProductRepository>();

            // Product Category
            services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();

            // Order
            services.AddTransient<IOrderApplication>(sp =>
    new OrderApplication(
        sp.GetRequiredService<IOrderRepository>(),
        sp.GetRequiredService<ICartRepository>(),
        sp.GetRequiredService<IProductRepository>(),
        sp.GetRequiredService<ShoppingSiteContext>()
    )
);
            services.AddTransient<IOrderRepository, OrderRepository>();

            // Order contact
            services.AddTransient<IOrderContactApplication, OrderContactApplication>();
            services.AddTransient<IOrderContactRepository, OrderContactRepository>();

            // Account
            services.AddTransient<IAccountApplication, AccountApplication>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            // Cart — اضافه شد: ShoppingSiteContext پارامتر چهارم
            services.AddTransient<ICartApplication, CartApplication>();
            services.AddTransient<ICartRepository, CartRepository>();

            // Settings
            services.AddTransient<ISettingsApplication, SettingsApplication>();
            services.AddTransient<ISiteSettingsRepository, SiteSettingsRepository>();

            services.AddTransient<FileUploader>();
            services.AddDbContext<ShoppingSiteContext>(x => x.UseSqlServer(con));
            //services.AddScoped(sp => sp.GetRequiredService<ShoppingSiteContext>());
            services.AddTransient<ICartApplication>(sp =>
    new CartApplication(
        sp.GetRequiredService<ICartRepository>(),
        sp.GetRequiredService<IProductRepository>(),
        sp.GetRequiredService<IOrderApplication>(),
        sp.GetRequiredService<ShoppingSiteContext>()
    )
);

        }
    }
}