using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    using Domain.Credits;
    using Domain.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class CreditConfiguration : IEntityTypeConfiguration<Credit>
    {
        public void Configure(EntityTypeBuilder<Credit> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Credit");

            builder.Ignore(e => e.Amount);

            builder.Property(credit => credit.Value)
                .IsRequired();

            builder.Property(credit => credit.Currency)
                .IsRequired();

            builder.Property(credit => credit.CreditId)
                .HasConversion(
                    value => value.Id,
                    value => new CreditId(value))
                .IsRequired();

            builder.Property(credit => credit.AccountId)
                .HasConversion(
                    value => value.Id,
                    value => new AccountId(value))
                .IsRequired();

            builder.Property(credit => credit.TransactionDate)
               .IsRequired();

            builder.Property(b => b.AccountId)
               .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
        }
    }
}
