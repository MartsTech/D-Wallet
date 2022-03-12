namespace Application.UseCases.Transactions.Transfer;

using Application.Core;
using Application.Services;
using Domain;
using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class TransferUseCase
{
    public class Command : IRequest<Result<DebitDto>?>
    {
        public Command(TransferInput input)
        {
            Input = input;
        }

        public TransferInput Input { get; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Input).SetValidator(new TransferInputValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<DebitDto>?>
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

        public Task<Result<DebitDto>?> Handle(Command request, CancellationToken cancellationToken)
        {
            return Transfer(
                new AccountId(request.Input.OriginAccountId),
                new AccountId(request.Input.DestinationAccountId),
                new Money(request.Input.Amount, new Currency(request.Input.Currency)));
        }

        private async Task<Result<DebitDto>?> Transfer(AccountId originAccountId, AccountId destinationAccountId, Money transferAmount)
        {
            IAccount originAccount = await _accountRepository
                .GetAccount(originAccountId)
                .ConfigureAwait(false);

            IAccount destinationAccount = await _accountRepository
                .GetAccount(destinationAccountId)
                .ConfigureAwait(false);

            if (originAccount is Account withdrawAccount && destinationAccount is Account depositAccount)
            {
                Money localCurrencyAmount =
                    await _currencyExchange
                        .Convert(transferAmount, withdrawAccount.Currency)
                        .ConfigureAwait(false);

                Debit debit = _entityFactory
                    .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

                if (withdrawAccount.GetCurrentBalance().Subtract(debit.Amount).Amount < 0)
                {
                    return Result<DebitDto>.Failure("Account does not have enough funds.");
                }

                await Withdraw(withdrawAccount, debit)
                    .ConfigureAwait(false);

                Money destinationCurrencyAmount = await _currencyExchange
                    .Convert(transferAmount, depositAccount.Currency)
                    .ConfigureAwait(false);

                Credit credit = _entityFactory
                    .NewCredit(depositAccount, destinationCurrencyAmount, DateTime.Now);

                await Deposit(depositAccount, credit)
                    .ConfigureAwait(false);

                int changes = await _unitOfWork
                    .Save()
                    .ConfigureAwait(false);

                return changes > 0 
                    ? Result<DebitDto>.Success(new DebitDto(debit))
                    : Result<DebitDto>.Failure("Failed to transfer amount");
            }

            return null;
        }

        private async Task Withdraw(Account account, Debit debit)
        {
            account.Withdraw(debit);

            await _accountRepository
                .Update(account, debit)
                .ConfigureAwait(false);
        }

        private async Task Deposit(Account account, Credit credit)
        {
            account.Deposit(credit);

            await _accountRepository
                .Update(account, credit)
                .ConfigureAwait(false);
        }
    }
}