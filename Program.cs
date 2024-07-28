using InvoicingSystem.Models;
using InvoicingSystem.Repositories;
using InvoicingSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the JSON file repositories
builder.Services.AddSingleton<IRepository<Product>>(sp => new JsonFileRepository<Product>("Data/products.json"));
builder.Services.AddSingleton<IRepository<Category>>(sp => new JsonFileRepository<Category>("Data/categories.json"));
builder.Services.AddSingleton<IRepository<Customer>>(sp => new JsonFileRepository<Customer>("Data/customers.json"));
builder.Services.AddSingleton<IRepository<Invoice>>(sp => new JsonFileRepository<Invoice>("Data/invoices.json"));

// Register the services
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IInvoiceService, InvoiceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
