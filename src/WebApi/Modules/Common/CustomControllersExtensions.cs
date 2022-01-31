namespace WebApi.Modules.Common;

using System.Text.Json;
using System.Text.Json.Serialization;
using FeatureFlags;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

public static class CustomControllersExtensions
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        IFeatureManager featureManager = services
           .BuildServiceProvider()
           .GetRequiredService<IFeatureManager>();

        bool isErrorFilterEnabled = featureManager
           .IsEnabledAsync(nameof(CustomFeature.ErrorFilter))
           .ConfigureAwait(false)
           .GetAwaiter()
           .GetResult();

        services
          .AddHttpContextAccessor()
          .AddMvc(opt =>
          {
              opt.OutputFormatters.RemoveType<TextOutputFormatter>();
              opt.OutputFormatters.RemoveType<StreamOutputFormatter>();
              opt.RespectBrowserAcceptHeader = true;

              if (isErrorFilterEnabled)
              {
                  opt.Filters.Add(new ExceptionFilter());
              }
          })
          .AddJsonOptions(opt =>
          {
              opt.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
              opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
              opt.JsonSerializerOptions.Converters.Add(
                  new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
          })
          .AddControllersAsServices();

        return services;
    }
}
