namespace Application.UseCases.Transfer;

using Application.Services;
using Domain.ValueObjects;

public sealed class TransferValidationUseCase : ITransferUseCase
{
    private readonly Notification _notification;
    private readonly ITransferUseCase _useCase;
    private IOutputPort _outputPort;

    public TransferValidationUseCase(Notification notification, ITransferUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
        _outputPort = new TransferPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    public async Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency)
    {
        if (originAccountId == Guid.Empty)
        {
            _notification.Add(nameof(originAccountId), "AccountId is required.");
        }

        if (destinationAccountId == Guid.Empty)
        {
            _notification.Add(nameof(destinationAccountId), "AccountId is required.");
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
            .Execute(originAccountId, destinationAccountId, amount, currency)
            .ConfigureAwait(false);
    }
}
