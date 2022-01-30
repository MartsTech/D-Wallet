namespace Application.UseCases.CloseAccount;

public interface ICloseAccountUseCase
{
    Task Execute(Guid accountId);

    void SetOutputPort(IOutputPort outputPort);
}
