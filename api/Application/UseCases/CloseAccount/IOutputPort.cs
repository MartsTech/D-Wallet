using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Accounts;


namespace Application.UseCases.CloseAccount;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void HasFunds();

    void Ok(Account account);
}
