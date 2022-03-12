namespace Application.UseCases.Accounts.OpenAccount;

using Domain.ValueObjects;
using FluentValidation;

public sealed class OpenAccountInputValidator : AbstractValidator<OpenAccountInput>
{
    public OpenAccountInputValidator()
    {
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