namespace Application.UseCases.Accounts.CloseAccount;

using FluentValidation;

public sealed class CloseAccountInputValidator : AbstractValidator<CloseAccountInput>
{
    public CloseAccountInputValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
    }
}