namespace IntegrationTests.EntityFrameworkTests;

using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

public sealed class StandardFixture : IDisposable
{
    public StandardFixture()
    {
        const string connectionString =
            "Server=localhost;User Id=root;Password=root;Database=d_wallet;";

        MySqlServerVersion serverVersion = new(new Version(8, 0, 27));

        DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;

        Context = new DataContext(options);
        Context.Database.EnsureCreated();
    }

    public DataContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
    }
}