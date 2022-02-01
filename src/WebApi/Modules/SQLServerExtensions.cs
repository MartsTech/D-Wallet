﻿namespace WebApi.Modules;

using Application.Services;
using WebApi.Modules.Common.FeatureFlags;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Domain.Accounts;
using Infrastructure.DataAccess.Factories;

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
                string connStr = config.GetConnectionString("DefaultConnection");

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));

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

        services.AddScoped<IAccountFactory, AccountFactory>();

        return services;
    }
}
