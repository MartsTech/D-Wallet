namespace Application.UseCases.Accounts.OpenAccount;

using Application.Core;
using Application.Services;
using Domain;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class OpenAccountUseCase
{
    public class Command : IRequest<Result<AccountDto>>
    {
        public Command(OpenAccountInput input)
        {
            Input = input;
        }

        public OpenAccountInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new OpenAccountInputValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<AccountDto>>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public Handler(
            IEntityFactory entityFactory,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _entityFactory = entityFactory;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public Task<Result<AccountDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            return OpenAccount(
                new Money(request.Input.Amount,
                new Currency(request.Input.Currency)));
        }

        private async Task<Result<AccountDto>> OpenAccount(Money deposit)
        {
            string userId = _userService.GetCurrentUserId();

            Account account = _entityFactory
                .NewAccount(userId, deposit.Currency);

            Credit credit = _entityFactory
                .NewCredit(account, deposit, DateTime.Now);

            bool success = await Deposit(account, credit)
                .ConfigureAwait(false);

            return success 
                ? Result<AccountDto>.Success(new AccountDto(account))
                : Result<AccountDto>.Failure("Failed to open an account.");
        }

        private async Task<bool> Deposit(Account account, Credit credit)
        {
            account.Deposit(credit);

            await _accountRepository
                .Add(account, credit)
                .ConfigureAwait(false);

            int changes = await _unitOfWork
                .Save()
                .ConfigureAwait(false);

            return changes > 0;
        }
    }
}
