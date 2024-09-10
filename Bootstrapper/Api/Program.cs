using Carter;
using Serilog;
using Shared.Exceptions.Handlers;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//common services: carter, mediatr, fluentvalidation
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly,basketAssembly)
    .AddMediatRWithAssemblies(catalogAssembly,basketAssembly);

//module services: catalog, basket, ordering
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.MapCarter();
app.UseExceptionHandler(options => {});

app.Run();
