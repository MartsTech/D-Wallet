namespace Application.UseCases.Transactions.Withdraw;

using Application.Core;
using Application.Services;
using Domain;
using Domain.Accounts;
using Domain.Debits;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class WithdrawUseCase
{
    public class Command : IRequest<Result<DebitDto>?>
    {
        public Command(WithdrawInput input)
        {
            Input = input;
        }

        public WithdrawInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new WithdrawInputValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<DebitDto>?>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public Handler(
            IEntityFactory entityFactory,
            IAccountRepository accountRepository,
            ICurrencyExchange currencyExchange,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _entityFactory = entityFactory;
            _accountRepository = accountRepository;
            _currencyExchange = currencyExchange;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public Task<Result<DebitDto>?> Handle(Command request, CancellationToken cancellationToken)
        {
            return Withdraw(
                new AccountId(request.Input.AccountId),
                new Money(request.Input.Amount, new Currency(request.Input.Currency)));
        }

        private async Task<Result<DebitDto>?> Withdraw(AccountId accountId, Money withdrawAmount)
        {
            string userId = _userService.GetCurrentUserId();

            IAccount account = await _accountRepository
                .Find(accountId, userId)
                .ConfigureAwait(false);

            if (account is Account withdrawAccount)
            {
                Money localCurrencyAmount = await _currencyExchange
                    .Convert(withdrawAmount, withdrawAccount.Currency)
                    .ConfigureAwait(false);

                Debit debit = _entityFactory
                    .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

                if (withdrawAccount.GetCurrentBalance().Subtract(debit.Amount).Amount < 0)
                {
                    return Result<DebitDto>.Failure("Account does not have enough funds.");
                }

                bool success = await Withdraw(withdrawAccount, debit)
                    .ConfigureAwait(false);

                return success
                    ? Result<DebitDto>.Success(new DebitDto(debit))
                    : Result<DebitDto>.Failure("Failed to withdraw amount.");
            }

            return null;
        }

        private async Task<bool> Withdraw(Account account, Debit debit)
        {
            account.Withdraw(debit);

            await _accountRepository
                .Update(account, debit)
                .ConfigureAwait(false);

            int changes = await _unitOfWork
                .Save()
                .ConfigureAwait(false);

            return changes > 0;
        }
    }
}