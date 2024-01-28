using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services
            .AddDbContext<AppDbContext>(o=>o.UseSqlite(configuration.GetConnectionString("DefaultConnection")))
            .AddSingleton<INetworkDataLoaderService, NetworkDataLoaderService>()
            .AddSingleton<IFileManipulationService, MacOSFileManipulationService>()
            .AddScoped<IDatabaseInitializer, SqlLiteDatabaseInitializer>();

            

        return services;
    }

}
