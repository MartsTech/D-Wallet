namespace WebApi.UseCases.Accounts.GetAccounts;

using Application.UseCases.Accounts;

public sealed class GetAccountsResponse
{
    public GetAccountsResponse(List<AccountDto> accounts)
    {
        Accounts = accounts;
    }

    public List<AccountDto> Accounts { get; } = new();
}