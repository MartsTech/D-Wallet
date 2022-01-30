using Application.Services;
using Application.UseCases.CloseAccount;
using Application.UseCases.Deposit;
using Application.UseCases.GetAccount;
using Application.UseCases.GetAccounts;
using Application.UseCases.OpenAccount;
using Application.UseCases.Transfer;
using Application.UseCases.Withdraw;

namespace WebApi.Modules;

public static class UseCasesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<Notification, Notification>();

        services.AddScoped<ICloseAccountUseCase, CloseAccountUseCase>();
        services.Decorate<ICloseAccountUseCase, CloseAccountValidationUseCase>();

        services.AddScoped<IDepositUseCase, DepositUseCase>();
        services.Decorate<IDepositUseCase, DepositValidationUseCase>();

        services.AddScoped<IGetAccountUseCase, GetAccountUseCase>();
        services.Decorate<IGetAccountUseCase, GetAccountValidationUseCase>();

        services.AddScoped<IGetAccountsUseCase, GetAccountsUseCase>();

        services.AddScoped<IOpenAccountUseCase, OpenAccountUseCase>();
        services.Decorate<IOpenAccountUseCase, OpenAccountValidationUseCase>();

        services.AddScoped<ITransferUseCase, TransferUseCase>();
        services.Decorate<ITransferUseCase, TransferValidationUseCase>();

        services.AddScoped<IWithdrawUseCase, WithdrawUseCase>();
        services.Decorate<IWithdrawUseCase, WithdrawValidationUseCase>();

        return services;
    }
}
