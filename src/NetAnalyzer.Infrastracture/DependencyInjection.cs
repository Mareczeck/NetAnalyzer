using Microsoft.Extensions.DependencyInjection;

namespace NetAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<INetworkDataLoaderService, NetworkDataLoaderService>()
            .AddSingleton<IFileManipulationService, MacOSFileManipulationService>()
            .AddSingleton<IDatabaseInitializer, SqlLiteDatabaseInitializer>();

            

        return services;
    }

}
