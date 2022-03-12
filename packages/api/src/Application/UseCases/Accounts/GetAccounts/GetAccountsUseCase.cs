namespace Application.UseCases.Accounts.GetAccounts;

using Application.Core;
using Application.Services;
using Domain.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public sealed class GetAccountsUseCase
{
    public class Query : IRequest<Result<IList<AccountDto>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<IList<AccountDto>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;

        public Handler(
            IAccountRepository accountRepository,
            IUserService userService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
        }

        public Task<Result<IList<AccountDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.GetCurrentUserId();

            return GetAccounts(userId);
        }

        private async Task<Result<IList<AccountDto>>> GetAccounts(string userId)
        {
            IList<Account> accounts = await _accountRepository
                .GetAccounts(userId)
                .ConfigureAwait(false);

            List<AccountDto> result = new(accounts.Count);

            foreach(Account account in accounts)
            {
                result.Add(new AccountDto(account));
            }

            return Result<IList<AccountDto>>.Success(result);
        }
    }
}