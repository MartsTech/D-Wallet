namespace WebApi.Modules.Common;

public static class CustomCorsExtensions
{
    private const string CorsPolicy = "CorsPolicy";

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(CorsPolicy, policy =>
            {
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("WWW-Authenticate")
                    .WithOrigins("http://localhost:5000");
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicy);
        return app;
    }
}