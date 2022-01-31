namespace WebApi.Modules.Common;

using Microsoft.AspNetCore.HttpOverrides;

public static class ReverseProxyExtensions
{
    public static IServiceCollection AddProxy(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(opt =>
        {
            opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    public static IApplicationBuilder UseProxy(
        this IApplicationBuilder app,
        IConfiguration config)
    {
        string basePath = config["ASPNETCORE_BASEPATH"];

        if (!string.IsNullOrEmpty(basePath))
        {
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = basePath;

                await next
                    .Invoke()
                    .ConfigureAwait(false);
            });
        }

        ForwardedHeadersOptions options = new() 
        { 
            ForwardedHeaders = ForwardedHeaders.All 
        };

        app.UseForwardedHeaders(options);

        return app;
    }
}
