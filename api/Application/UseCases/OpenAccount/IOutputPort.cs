using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.OpenAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Ok(Account account);

    void NotFound();

    void Invalid();
}
