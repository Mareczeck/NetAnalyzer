﻿using Microsoft.Extensions.DependencyInjection;

namespace NetAnalyzer.Business;

public static class DependencyInjection
{

    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services
            .AddScoped<IGraphProcessor,GraphProcessor>()
            .AddScoped<IDatasetService,DatasetService>();

        return services;
    }

}
