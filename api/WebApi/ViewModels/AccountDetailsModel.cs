using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.ViewModels;

using Domain.Accounts;
using System.ComponentModel.DataAnnotations;

public sealed class AccountDetailsModel
{
    public AccountDetailsModel(Account account)
    {
        AccountId = account.AccountId.Id;

        CurrentBalance = account.GetCurrentBalance().Amount;

        Credits = account
             .CreditsCollection
             .Select(e => new CreditModel(e))
             .ToList();

        Debits = account
             .DebitsCollection
             .Select(e => new DebitModel(e))
             .ToList();
    }

    [Required]
    public Guid AccountId { get; }

    [Required]
    public decimal CurrentBalance { get; }

    [Required]
    public List<CreditModel> Credits { get; }

    [Required]
    public List<DebitModel> Debits { get; }
}
