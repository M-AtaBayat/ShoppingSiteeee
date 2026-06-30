using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_ShoppingSiteQuery.Contracts.Product;
using _01_ShoppingSiteQuery.Queries;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShoppingSiteManagement.Application.AccountAPP;
using ShoppingSiteManagement.Application.Contracts.AccountAPC;
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
using ShoppingSiteManagement.Domain.OrderAgg;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using ShoppingSiteManagement.Domain.ProductAgg;
using ShoppingSiteManagement.Domain.ProductCategoryAgg;
using ShoppingSiteManagement.Domain.SettingsAgg;
using ShoppingSiteManagement.Infrastructure.Config;
using ShoppingSiteManagement.Infrastructure.EFCore;
using ShoppingSiteManagement.Infrastructure.EFCore.Repository;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

ShoppingSiteBootstrapper.ShoppingSiteConfig(builder.Services, connectionString);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Admin", "/");
    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Login");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = "/Admin/Login";
        o.Cookie.Name = "MyProjectAuth";
        o.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

//builder.Services.AddDbContext<ShoppingSiteContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IAccountApplication, AccountApplication>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IOrderApplication, OrderApplication>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
builder.Services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddTransient<IProductApplication, ProductApplication>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ISettingsApplication, SettingsApplication>();
builder.Services.AddTransient<ISiteSettingsRepository, SiteSettingsRepository>();
builder.Services.AddTransient<IOrderContactApplication, OrderContactApplication>();
builder.Services.AddTransient<IOrderContactRepository, OrderContactRepository>();
builder.Services.AddTransient<IProductQuery, ProductQuery>();
builder.Services.AddTransient<FileUploader>();





var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
}

app.Run();