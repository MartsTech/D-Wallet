namespace Persistence.Factories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public sealed class ContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        string connStr = ReadDefaultConnectionStringFromAppSettings();

        DbContextOptionsBuilder<DataContext> builder = new();

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));

        builder.UseMySql(connStr, serverVersion);

        return new DataContext(builder.Options);
    }

    private static string ReadDefaultConnectionStringFromAppSettings()
    {
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{env}.json", false)
            .AddEnvironmentVariables()
            .Build();

        string connStr = config.GetConnectionString("DefaultConnection");

        return connStr;
    }
}