namespace Infrastructure.DataAccess.Configurations;

using Domain.Debits;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class DebitConfiguration : IEntityTypeConfiguration<Debit>
{
    public void Configure(EntityTypeBuilder<Debit> builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ToTable("Debit");

        builder.Ignore(e => e.Amount);

        builder.Property(debit => debit.Value)
           .IsRequired();

        builder.Property(debit => debit.Currency)
           .IsRequired();

        builder.Property(debit => debit.DebitId)
            .HasConversion(
                value => value.Id,
                value => new DebitId(value))
            .IsRequired();

        builder.Property(debit => debit.AccountId)
          .HasConversion(
              value => value.Id,
              value => new AccountId(value))
          .IsRequired();

        builder.Property(debit => debit.TransactionDate)
            .IsRequired();

        builder.Property(b => b.AccountId)
            .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
    }
}
