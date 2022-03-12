namespace WebApi.UseCases.Accounts.CloseAccount;

public sealed class CloseAccountResponse
{
    public CloseAccountResponse(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; }
}