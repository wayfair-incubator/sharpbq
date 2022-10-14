using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sharpbq.Configuration;
using sharpbq.DataAccess;

namespace sharpbq.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBigQueryClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IBigQueryClientFactory, BigQueryClientFactory>();
        services.AddTransient<IDataStoreBase, DataStoreBase>();
        services.Configure<BigQueryProjectSettings>(options => configuration.GetSection("BigQuery").Bind(options));
    }
}

