using Application.Services;
using Domain.ValueObjects;

namespace Application.UseCases.Deposit;

public sealed class DepositValidationUseCase : IDepositUseCase
{
    private readonly Notification _notification;
    private readonly IDepositUseCase _useCase;
    private IOutputPort _outputPort;

    public DepositValidationUseCase(Notification notification, IDepositUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
        _outputPort = new DepositPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    public async Task Execute(Guid accountId, decimal amount, string currency)
    {
        if (accountId == Guid.Empty)
        {
            _notification.Add(nameof(accountId), "AccountId is required.");
        }

        if (currency != Currency.Dollar.Code &&
         currency != Currency.Euro.Code &&
         currency != Currency.BritishPound.Code &&
         currency != Currency.Canadian.Code &&
         currency != Currency.Real.Code &&
         currency != Currency.Krona.Code)
        {
            _notification.Add(nameof(currency), "Currency is required.");
        }

        if (amount <= 0)
        {
            _notification.Add(nameof(amount), "Amount should be positive.");
        }

        if (_notification.IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await _useCase
            .Execute(accountId, amount, currency)
            .ConfigureAwait(false);
    }
}
