using Application.Services;
using Domain.ValueObjects;

namespace Application.UseCases.OpenAccount;

public sealed class OpenAccountValidationUseCase : IOpenAccountUseCase
{
    private readonly Notification _notification;
    private readonly IOpenAccountUseCase _useCase;
    private IOutputPort _outputPort;

    public OpenAccountValidationUseCase(Notification notification, IOpenAccountUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
        _outputPort = new OpenAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    public async Task Execute(decimal amount, string currency)
    {
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
           .Execute(amount, currency)
           .ConfigureAwait(false);
    }
}
