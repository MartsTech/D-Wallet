using Application.Services;
using Domain.Accounts;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Factories;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using WebApi.Modules.Common.FeatureFlags;

namespace WebApi.Modules;

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
            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(
                    config.GetValue<string>("PersistenceModule:DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountRepository, AccountRepository>();
        }
        else
        {
            services.AddSingleton<DataContextFake, DataContextFake>();
            services.AddScoped<IUnitOfWork, UnitOfWorkFake>();
            services.AddScoped<IAccountRepository, AccountRepositoryFake>();
        }

        services.AddScoped<IAccountFactory, AccountFactory>();

        return services;
    }
}
