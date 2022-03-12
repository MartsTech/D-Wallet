namespace Application.UseCases.Accounts.GetAccount;

using Application.Core;
using Domain.Accounts;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public sealed class GetAccountUseCase
{
    public class Query : IRequest<Result<AccountDetailsDto>?>
    {
        public Query(GetAccountInput input)
        {
            Input = input;
        }

        public GetAccountInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Query>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new GetAccountInputValidator());
        }
    }

    public class Handler : IRequestHandler<Query, Result<AccountDetailsDto>?>
    {
        private readonly IAccountRepository _accountRepository;

        public Handler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<Result<AccountDetailsDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            return GetAccount(new AccountId(request.Input.AccountId));
        }

        private async Task<Result<AccountDetailsDto>?> GetAccount(AccountId accountId)
        {
            IAccount account = await _accountRepository
                .GetAccount(accountId)
                .ConfigureAwait(false);

            if (account is Account getAccount)
            {
                return Result<AccountDetailsDto>.Success(new AccountDetailsDto(getAccount));
            }

            return null;
        }
    }
}