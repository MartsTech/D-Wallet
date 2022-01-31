using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using WebApi.Modules.Common.FeatureFlags;

namespace WebApi.Modules.Common;

public static class SwaggerExtensions
{
    private static string XmlCommentsFilePath
    {
        get
        {
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            string fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }

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
                .AddSwaggerGen(
                    c =>
                    {
                        c.IncludeXmlComments(XmlCommentsFilePath);
                        c.AddSecurityDefinition("Bearer",
                            new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Description = "Please insert JWT with Bearer into field",
                                Name = "Authorization",
                                Type = SecuritySchemeType.ApiKey
                            });
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                        }
                                    },
                                    new string[] { }
                                }
                        });
                    });
        }

        return services;

    }
}
