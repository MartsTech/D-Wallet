namespace WebApi.UseCases.Accounts.GetAccount;

using System.ComponentModel.DataAnnotations;

public sealed class GetAccountDetailsRequest
{
    [Required]
    public Guid AccountId { get; set; }
}
