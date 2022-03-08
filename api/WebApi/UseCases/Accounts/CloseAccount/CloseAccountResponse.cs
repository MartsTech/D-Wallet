using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.CloseAccount;

using Domain.Accounts;
using System.ComponentModel.DataAnnotations;

public sealed class CloseAccountResponse
{
    public CloseAccountResponse(Account account)
    {
        AccountId = account.AccountId.Id;
    }

    [Required]
    public Guid AccountId { get; }
}
