namespace Application.UseCases.Accounts.GetAccount;

using System.ComponentModel.DataAnnotations;

public sealed class GetAccountInput
{
    [Required]
    public Guid AccountId { get; set; }
}