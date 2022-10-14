using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sharpbq.Configuration;
using sharpbq.DataAccess;
using sharpbq.DataAccess.Clients;

namespace sharpbq.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBigQueryClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ISharpBQClient, SharpBQClient>();
        services.AddTransient<IBigQueryClientFactory, BigQueryClientFactory>();
        services.AddTransient<IDataStoreBase, DataStoreBase>();
        services.Configure<BigQueryProjectSettings>(options => configuration.GetSection("BigQuery").Bind(options));
    }
}

