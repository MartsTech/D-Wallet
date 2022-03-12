namespace WebApi.Modules.Common.FeatureFlags;

using Microsoft.FeatureManagement;

public static class FeatureFlagsExtensions
{
    public static IServiceCollection AddFeatureFlags(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddFeatureManagement(config);

        IFeatureManager featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        services
            .AddMvc()
            .ConfigureApplicationPartManager(apm =>
            {
                apm.FeatureProviders.Add(
                    new CustomControllerFeatureProvider(featureManager));
            });


        return services;
    }
}