using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.GetAccounts;

using Domain.Accounts;
using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class GetAccountsResponse
{
    public GetAccountsResponse(IEnumerable<Account> accounts)
    {
        foreach (Account account in accounts)
        {
            AccountModel accountModel = new(account);
            Accounts.Add(accountModel);
        }
    }

    [Required]
    public List<AccountModel> Accounts { get; } = new List<AccountModel>();
}
