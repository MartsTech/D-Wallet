namespace Application.UseCases.Transactions.Transfer;

using Domain.ValueObjects;
using FluentValidation;

public sealed class TransferInputValidator : AbstractValidator<TransferInput>
{
    public TransferInputValidator()
    {
        RuleFor(x => x.OriginAccountId).NotEmpty();
        RuleFor(x => x.DestinationAccountId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).Must(x =>
            x == Currency.Euro.Code ||
            x == Currency.Dollar.Code ||
            x == Currency.Lev.Code ||
            x == Currency.Pound.Code ||
            x == Currency.Real.Code ||
            x == Currency.Krona.Code);
    }
}