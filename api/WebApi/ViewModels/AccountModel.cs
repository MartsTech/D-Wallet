using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.ViewModels;

using Domain.Accounts;
using System.ComponentModel.DataAnnotations;

public sealed class AccountModel
{
    public AccountModel(Account account)
    {
        AccountId = account.AccountId.Id;

        CurrentBalance = account
            .GetCurrentBalance()
            .Amount;

        Currency = account.Currency.Code;
    }

    [Required]
    public Guid AccountId { get; }

    [Required]
    public decimal CurrentBalance { get; }

    [Required]
    public string Currency { get; }
}
