using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Infrastructure.Repositories;

namespace SlagFieldManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        return services;
    }

    private static void AddPersistence(
        IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("LocalDatabase") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IBucketRepository, BucketRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IMaterialSettingsRepository, MaterialSettingsRepository>();
        services.AddScoped<ISlagFieldPlaceRepository, SlagFieldPlaceRepository>();
        services.AddScoped<ISlagFieldStateRepository, SlagFieldStateRepository>();
        services.AddScoped<ISlagFieldStockRepository, SlagFieldStockRepository>();

        services.AddScoped<ISlagFieldQueryRepository, SlagFieldQueryRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        services.AddScoped<IPlaceEventStore, SlagFieldPlaceEventStore>();
        services.AddScoped<IStateEventStore, SlagFieldStateEventStore>();
    }
}