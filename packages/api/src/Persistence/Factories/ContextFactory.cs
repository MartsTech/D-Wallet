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

        string connStr;

        if (env == "Development")
        {
            connStr = config.GetConnectionString("DefaultConnection");
        }
        else
        {
            string? mysqlHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
            string? mysqlPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
            string? mysqlUser = Environment.GetEnvironmentVariable("MYSQL_USER");
            string? mysqlPass = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            string? mysqlDb = Environment.GetEnvironmentVariable("MYSQL_DATABASE");

            connStr = $"Server={mysqlHost};Port={mysqlPort};User Id={mysqlUser};Password={mysqlPass};Database={mysqlDb};";
        }
        
        return connStr;
    }
}