namespace Application.UseCases.Accounts;

using Domain.Accounts;

public sealed class AccountDto
{
    public AccountDto(Account account)
    {
        Id = account.AccountId.Id;
        CurrentBalance = account.GetCurrentBalance().Amount;
        Currency = account.Currency.Code;
    }

    public Guid Id { get; }

    public decimal CurrentBalance { get; }

    public string Currency { get; }
}