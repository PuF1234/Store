﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    public static class ServiceCollectionExtencions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                },
                ServiceLifetime.Transient
            );

            services.AddSingleton<IBicycleRepository, BicycleRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
