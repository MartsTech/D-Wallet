using Application.Services;

namespace Application.UseCases.CloseAccount;

public sealed class CloseAccountValidationUseCase : ICloseAccountUseCase
{
    private readonly Notification _notification;
    private readonly ICloseAccountUseCase _useCase;
    private IOutputPort _outputPort;

    public CloseAccountValidationUseCase(Notification notification, ICloseAccountUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
        _outputPort = new CloseAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    public async Task Execute(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            _notification.Add(nameof(accountId), "AccountId is required.");
        }

        if (!_notification.IsValid)
        {
            _outputPort.Invalid();
            return;
        }

        await _useCase
            .Execute(accountId)
            .ConfigureAwait(false);
    }
}
