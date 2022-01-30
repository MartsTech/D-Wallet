namespace Application.UseCases.GetAccounts;

public interface IGetAccountsUseCase
{
    Task Execute();

    void SetOutputPort(IOutputPort outputPort);
}
