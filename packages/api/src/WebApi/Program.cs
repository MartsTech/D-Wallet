using Microsoft.EntityFrameworkCore;
using Persistence;
using WebApi.Modules;
using WebApi.Modules.Common;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.Modules.Common.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
      .AddFeatureFlags(builder.Configuration)
      .AddInvalidRequestLogging()
      .AddCurrencyExchange(builder.Configuration)
      .AddSQLServer(builder.Configuration)
      .AddAuthentication(builder.Configuration)
      .AddSwagger()
      .AddUseCases()
      .AddCustomControllers()
      .AddCustomCors();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app
    .UseCustomCors()
    .UseRouting()
    .UseVersionedSwagger()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error ocurred during migration.");
}

await app.RunAsync();