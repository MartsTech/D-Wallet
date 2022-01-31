namespace Application.UseCases.Deposit;

public interface IDepositUseCase
{
    Task Execute(Guid accountId, decimal amount, string currency);

    void SetOutputPort(IOutputPort outputPort);
}
