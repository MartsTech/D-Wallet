using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.OpenAccount;

using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class OpenAccountResponse
{
    public OpenAccountResponse(AccountModel account)
    {
        Account = account;
    }

    [Required]
    public AccountModel Account { get; }
}