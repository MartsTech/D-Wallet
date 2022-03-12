namespace WebApi.UseCases.Accounts.GetAccount;

using Application.UseCases.Accounts;

public sealed class GetAccountResponse
{
    public GetAccountResponse(AccountDetailsDto account)
    {
        Account = account;
    }

    public AccountDetailsDto Account { get; }
}