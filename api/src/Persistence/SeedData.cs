namespace Persistence;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static readonly string DefaultExternalUserId = "df1525ce-1f1b-4e22-81fd-1065a35e4d5c";

    public static readonly AccountId DefaultAccountId = new(new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"));

    public static readonly AccountId SecondAccountId = new(new Guid("4BE769CC-8FDC-4678-B73F-0FBFA5EFF466"));

    public static readonly string SecondExternalUserId = "D20DB903-E69C-4F2B-9738-06F8CA2BD062";

    public static readonly CreditId DefaultCreditId = new(new Guid("A86F8863-099F-49B2-ACEC-274476CB559D"));

    public static readonly DebitId DefaultDebitId = new(new Guid("3B31A10F-A9FE-49AD-94CB-AD32C07D13CB"));

    public static void Seed(ModelBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Entity<Account>()
           .HasData(
               new
               {
                   AccountId = DefaultAccountId,
                   ExternalUserId = DefaultExternalUserId,
                   Currency = Currency.Dollar
               });

        builder.Entity<Credit>()
           .HasData(
               new
               {
                   CreditId = DefaultCreditId,
                   AccountId = DefaultAccountId,
                   TransactionDate = DateTime.UtcNow,
                   Value = 400m,
                   Currency = Currency.Dollar.Code
               });

        builder.Entity<Debit>()
           .HasData(
               new
               {
                   DebitId = DefaultDebitId,
                   AccountId = DefaultAccountId,
                   TransactionDate = DateTime.UtcNow,
                   Value = 50m,
                   Currency = Currency.Dollar.Code
               });
    }
}