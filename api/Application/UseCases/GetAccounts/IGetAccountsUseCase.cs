using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetAccounts;

public interface IGetAccountsUseCase
{
    Task Execute();

    void SetOutputPort(IOutputPort outputPort);
}
