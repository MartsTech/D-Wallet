namespace Application.UseCases.Transfer;

public interface ITransferUseCase
{
    Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency);

    void SetOutputPort(IOutputPort outputPort);
}
