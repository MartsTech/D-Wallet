namespace Application.UseCases.Accounts.CloseAccount;

using Application.Core;
using Application.Services;
using Domain.Accounts;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public sealed class CloseAccountUseCase
{
    public class Command : IRequest<Result<Guid>?>
    {
        public Command(CloseAccountInput input)
        {
            Input = input;
        }

        public CloseAccountInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new CloseAccountInputValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Guid>?>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public Handler(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public Task<Result<Guid>?> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = _userService.GetCurrentUserId();

            return CloseAccount(
                new AccountId(request.Input.AccountId),
                userId);
        }

        private async Task<Result<Guid>?> CloseAccount(AccountId accountId, string userId)
        {
            IAccount account = await _accountRepository
                .Find(accountId, userId)
                .ConfigureAwait(false);

            if (account is Account closingAccount)
            {
                if (!closingAccount.IsClosingAllowed())
                {
                    return Result<Guid>.Failure("Account has funds.");
                }

                bool success = await Close(closingAccount)
                    .ConfigureAwait(false);

                return success
                    ? Result<Guid>.Success(closingAccount.AccountId.Id)
                    : Result<Guid>.Failure("Failed to close account.");
            }

            return null;
        }

        private async Task<bool> Close(Account closeAccount)
        {
            await _accountRepository
                .Delete(closeAccount.AccountId)
                .ConfigureAwait(false);

            int changes = await _unitOfWork
                .Save()
                .ConfigureAwait(false);

            return changes > 0;
        }
    }
}