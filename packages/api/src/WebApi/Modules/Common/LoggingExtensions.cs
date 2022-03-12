namespace WebApi.Modules.Common;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public static class LoggingExtensions
{
    public static IServiceCollection AddInvalidRequestLogging(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = actionContext =>
            {
                ILogger<Program>? logger = actionContext
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<ILogger<Program>>();

                List<string> errors = actionContext.ModelState
                    .Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                string jsonModelState = JsonSerializer.Serialize(errors);

                logger.LogWarning("Invalid request.", jsonModelState);

                ValidationProblemDetails problemDetails = new(actionContext.ModelState);

                return new BadRequestObjectResult(problemDetails);
            };
        });

        return services;
    }
}