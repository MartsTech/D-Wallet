namespace Infrastructure.DataAccess.Factories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public sealed class ContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        string connectionString = ReadDefaultConnectionStringFromAppSettings();

        DbContextOptionsBuilder<DataContext> builder = new();

        builder.UseSqlServer(connectionString);

        return new DataContext(builder.Options);
    }

    private static string ReadDefaultConnectionStringFromAppSettings()
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{env}.json", false)
            .AddEnvironmentVariables()
            .Build();

        string connectionString = configuration.GetValue<string>("PersistenceModule:DefaultConnection");

        return connectionString;
    }
}
