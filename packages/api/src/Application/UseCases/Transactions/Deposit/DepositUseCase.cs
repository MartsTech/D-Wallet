namespace Application.UseCases.Transactions.Deposit;

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

public sealed class DepositUseCase
{
    public class Command : IRequest<Result<CreditDto>?>
    {
        public Command(DepositInput input)
        {
            Input = input;
        }

        public DepositInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new DepositInputValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<CreditDto>?>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IEntityFactory entityFactory,
            IAccountRepository accountRepository,
            ICurrencyExchange currencyExchange,
            IUnitOfWork unitOfWork)
        {
            _entityFactory = entityFactory;
            _accountRepository = accountRepository;
            _currencyExchange = currencyExchange;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<CreditDto>?> Handle(Command request, CancellationToken cancellationToken)
        {
            return Deposit(
                new AccountId(request.Input.AccountId),
                new Money(request.Input.Amount, new Currency(request.Input.Currency)));
        }

        private async Task<Result<CreditDto>?> Deposit(AccountId accountId, Money amount)
        {
            IAccount account = await _accountRepository
                .GetAccount(accountId)
                .ConfigureAwait(false);

            if (account is Account depositAccount)
            {
                Money convertedAmount = await _currencyExchange
                    .Convert(amount, depositAccount.Currency)
                    .ConfigureAwait(false);

                Credit credit = _entityFactory
                    .NewCredit(depositAccount, convertedAmount, DateTime.Now);

                bool success = await Deposit(depositAccount, credit)
                    .ConfigureAwait(false);

                return success
                    ? Result<CreditDto>.Success(new CreditDto(credit))
                    : Result<CreditDto>.Failure("Failed to deposit amount.");
            }

            return null;
        }

        private async Task<bool> Deposit(Account account, Credit credit)
        {
            account.Deposit(credit);

            await _accountRepository
                .Update(account, credit)
                .ConfigureAwait(false);

            int changes = await _unitOfWork
                .Save()
                .ConfigureAwait(false);

            return changes > 0;
        }
    }
}