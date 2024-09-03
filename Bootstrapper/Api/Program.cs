using Carter;
using Serilog;
using Shared.Exceptions.Handlers;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));


builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

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
