using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Factories;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Credit> Credits { get; set; }

    public DbSet<Debit> Debits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        SeedData.Seed(modelBuilder);
    }
}
