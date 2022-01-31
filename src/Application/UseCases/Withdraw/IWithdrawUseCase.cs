namespace Application.UseCases.Withdraw;

public interface IWithdrawUseCase
{
    Task Execute(Guid accountId, decimal amount, string currency);

    void SetOutputPort(IOutputPort outputPort);
}
