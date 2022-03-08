using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetAccount;

public interface IGetAccountUseCase
{
    Task Execute(Guid accountId);

    void SetOutputPort(IOutputPort outputPort);
}
