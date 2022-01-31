namespace WebApi.Modules.Common.Swagger;

using FeatureFlags;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        IFeatureManager featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
           .IsEnabledAsync(nameof(CustomFeature.Swagger))
           .ConfigureAwait(false)
           .GetAwaiter()
           .GetResult();

        if (isEnabled)
        {
            services
                .AddSwaggerGen(opt =>
                {
                    opt.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please insert JWT with Bearer into field",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey
                        });
                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });
        }

        return services;
    }

    public static IApplicationBuilder UseVersionedSwagger(
      this IApplicationBuilder app,
      IApiVersionDescriptionProvider provider,
      IConfiguration config,
      IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                string swaggerEndpoint;

                string basePath = config["ASPNETCORE_BASEPATH"];

                if (!string.IsNullOrEmpty(basePath))
                {
                    swaggerEndpoint = $"{basePath}/swagger/{description.GroupName}/swagger.json";
                }
                else
                {
                    swaggerEndpoint = $"/swagger/{description.GroupName}/swagger.json";
                }

                opt.SwaggerEndpoint(swaggerEndpoint, description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}
