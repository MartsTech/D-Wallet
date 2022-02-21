namespace Persistence.Configurations;

using Domain.Accounts;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ToTable("Account");

        builder.Property(b => b.AccountId)
           .HasConversion(
               v => v.Id,
               v => new AccountId(v))
           .IsRequired();

        builder.Property(b => b.ExternalUserId)
           .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

        builder.Property(credit => credit.Currency)
          .HasConversion(
              value => value.Code,
              value => new Currency(value))
          .IsRequired();

        builder.HasMany(x => x.CreditsCollection)
            .WithOne(b => b.Account)
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DebitsCollection)
            .WithOne(b => b.Account)
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
