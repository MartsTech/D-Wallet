using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void Ok(Account account);
}
