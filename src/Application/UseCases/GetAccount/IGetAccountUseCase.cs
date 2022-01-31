namespace Application.UseCases.GetAccount;

public interface IGetAccountUseCase
{
    Task Execute(Guid accountId);

    void SetOutputPort(IOutputPort outputPort);
}
