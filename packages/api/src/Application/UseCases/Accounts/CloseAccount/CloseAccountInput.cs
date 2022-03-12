namespace Application.UseCases.Accounts.CloseAccount;

using System.ComponentModel.DataAnnotations;

public sealed class CloseAccountInput
{
    [Required]
    public Guid AccountId { get; set; }
}