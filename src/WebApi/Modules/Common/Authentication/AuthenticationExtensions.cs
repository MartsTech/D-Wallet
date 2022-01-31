namespace WebApi.Modules.Common.Authentication;

using Application.Services;
using Infrastructure.ExternalAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.FeatureManagement;
using WebApi.Modules.Common.FeatureFlags;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services, 
        IConfiguration config)
    {
        IFeatureManager featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeature.Authentication))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            services.AddScoped<IUserService, ExternalUserService>();

            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", opt =>
                {
                    opt.Authority = config["AuthenticationModule:AuthorityUrl"];
                    opt.ApiName = "api1";

                    opt.RequireHttpsMetadata = false;
                });
        }
        else
        {
            services.AddScoped<IUserService, TestUserService>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = "Test";
                opt.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                "Test", opt => { });
        }

        return services;
    }
}