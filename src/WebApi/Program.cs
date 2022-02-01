using WebApi.Modules;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.Modules.Common.Swagger;
using WebApi.Modules.Common.Authentication;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);

builder.Services
      .AddFeatureFlags(builder.Configuration)
      .AddInvalidRequestLogging()
      .AddCurrencyExchange(builder.Configuration)
      .AddSQLServer(builder.Configuration)
      .AddHealthChecks(builder.Configuration)
      .AddAuthentication(builder.Configuration)
      .AddVersioning()
      .AddSwagger()
      .AddUseCases()
      .AddCustomControllers()
      .AddCustomCors()
      .AddProxy()
      .AddCustomDataProtection();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/api/CustomError").UseHsts();
}

app
    .UseProxy(app.Configuration)
    .UseHealthChecks()
    .UseCustomCors()
    .UseCustomHttpMetrics()
    .UseRouting()
    .UseVersionedSwagger(app.Services, app.Configuration)
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapMetrics();
    });

app.Run();
