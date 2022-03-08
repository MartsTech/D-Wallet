using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.OpenAccount;

public interface IOpenAccountUseCase
{
    Task Execute(decimal amount, string currency);

    void SetOutputPort(IOutputPort outputPort);
}
