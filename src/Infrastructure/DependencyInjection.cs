﻿using Domain.Interfaces;
using Dorssel.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        // Configure SQLite
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseSqlite("Data Source=calendar.db")
                .UseSqliteTimestamp());
        
        services.AddScoped<IEventRepository, EventRepository>();
    }
}
