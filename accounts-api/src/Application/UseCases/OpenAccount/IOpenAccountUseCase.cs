namespace Application.UseCases.OpenAccount;

public interface IOpenAccountUseCase
{
    Task Execute(decimal amount, string currency);

    void SetOutputPort(IOutputPort outputPort);
}
