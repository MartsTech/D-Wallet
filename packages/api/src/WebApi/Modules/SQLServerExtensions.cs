namespace WebApi.Modules;

using Application.Services;
using WebApi.Modules.Common.FeatureFlags;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Domain.Accounts;
using Persistence;
using Persistence.Repositories;
using Domain;
using Persistence.Factories;

public static class SQLServerExtensions
{
    public static IServiceCollection AddSQLServer(this IServiceCollection services, IConfiguration config)
    {
        IFeatureManager featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeature.SQLServer))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr;

                if (env == "Development")
                {
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    var mysqlHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
                    var mysqlPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
                    var mysqlUser = Environment.GetEnvironmentVariable("MYSQL_USER");
                    var mysqlPass = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
                    var mysqlDb = Environment.GetEnvironmentVariable("MYSQL_DATABASE");

                    connStr = $"Server={mysqlHost};Port={mysqlPort};User Id={mysqlUser};Password={mysqlPass};Database={mysqlDb};";
                }

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

                opt.UseMySql(connStr, serverVersion);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
        else
        {
            services.AddSingleton<DataContextFake, DataContextFake>();
            services.AddScoped<IUnitOfWork, UnitOfWorkFake>();
            services.AddScoped<IAccountRepository, AccountRepositoryFake>();
        }

        services.AddScoped<IEntityFactory, EntityFactory>();

        return services;
    }
}