namespace WebApi.Modules;

using Application.UseCases.Accounts.OpenAccount;
using MediatR;

public static class UseCasesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(typeof(OpenAccountUseCase.Handler).Assembly);

        return services;
    }
}