using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.CloseAccount;

public interface ICloseAccountUseCase
{
    Task Execute(Guid accountId);

    void SetOutputPort(IOutputPort outputPort);
}
