using iniskincare.DataAccess;
using iniskincare.DataAccess.Repositories;
using iniskincare.Domain.Cart;
using iniskincare.Domain.Entities;
using iniskincare.Domain.Interfaces;
using iniskincare.Domain.Interfaces.Cart;
using iniskincare.Domain.Interfaces.IProduct;
using iniskincare.Domain.IProduct;
using iniskincare.Service;
using iniskincare.Service.Cart;
using iniskincare.Service.ProductService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IniskincareDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("ProductPage")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

