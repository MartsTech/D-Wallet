namespace Application.UseCases.Accounts;

using Application.UseCases.Transactions;
using Domain.Accounts;

public sealed class AccountDetailsDto
{
    public AccountDetailsDto(Account account)
    {
        Id = account.AccountId.Id;
        CurrentBalance = account.GetCurrentBalance().Amount;
        Credits = account
            .CreditsCollection
            .Select(e => new CreditDto(e))
            .ToList();
        Debits = account
            .DebitsCollection
            .Select(e => new DebitDto(e))
            .ToList();
    }

    public Guid Id { get; }

    public decimal CurrentBalance { get; }

    public List<CreditDto> Credits { get; }

    public List<DebitDto> Debits { get; }
}