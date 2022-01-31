namespace Application.UseCases.GetAccount;

using Application.Services;

public sealed class GetAccountValidationUseCase : IGetAccountUseCase
{
    private readonly Notification _notification;
    private readonly IGetAccountUseCase _useCase;
    private IOutputPort _outputPort;

    public GetAccountValidationUseCase(Notification notification, IGetAccountUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
        _outputPort = new GetAccountPresenter();
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

        if (_notification.IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await _useCase
            .Execute(accountId)
            .ConfigureAwait(false);
    }
}
