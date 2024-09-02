
using Carter;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.MapCarter();

app.Run();
