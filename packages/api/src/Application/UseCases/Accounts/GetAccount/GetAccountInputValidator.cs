namespace Application.UseCases.Accounts.GetAccount;

using FluentValidation;

public sealed class GetAccountInputValidator : AbstractValidator<GetAccountInput>
{
    public GetAccountInputValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
    }
}