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

app.Run();
