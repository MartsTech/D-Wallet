using Application.Services;
using Infrastructure.CurrencyExchange;
using Microsoft.FeatureManagement;
using WebApi.Modules.Common.FeatureFlags;

namespace WebApi.Modules;

public static class CurrencyExchangeExtensions
{
    public static IServiceCollection AddCurrencyExchange(this IServiceCollection services, IConfiguration config)
    {
        IFeatureManager featureManager = services
           .BuildServiceProvider()
           .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeature.CurrencyExchange))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            services.AddHttpClient(CurrencyExchangeService.HttpClientName);
            services.AddScoped<ICurrencyExchange, CurrencyExchangeService>();
        }
        else
        {
            services.AddScoped<ICurrencyExchange, CurrencyExchangeFake>();
        }

        return services;
    }
}