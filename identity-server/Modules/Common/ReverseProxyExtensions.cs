using IdentityServer4.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

namespace IdentityServer.Modules.Common;

public static class ReverseProxyExtensions
{
    public static IServiceCollection AddProxy(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    public static IApplicationBuilder UseProxy(this IApplicationBuilder app, IConfiguration config)
    {
        var identityServerOrigin = config["IDENTITY_SERVER_ORIGIN"];
        var basePath = config["ASPNETCORE_BASEPATH"];

        if (!string.IsNullOrEmpty(identityServerOrigin))
        {
            app.Use(async (context, next) =>
            {
                context.SetIdentityServerOrigin(identityServerOrigin);
                context.Request.PathBase = basePath;

                await next.Invoke().ConfigureAwait(false);
            });
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

        return app;
    }
}
