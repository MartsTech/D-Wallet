using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.GetAccount;

using Domain.Accounts;
using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class GetAccountResponse
{
    public GetAccountResponse(Account account)
    {
        Account = new AccountDetailsModel(account);
    }

    [Required]
    public AccountDetailsModel Account { get; }
}
