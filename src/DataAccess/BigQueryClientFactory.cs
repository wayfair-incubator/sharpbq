using Microsoft.Extensions.Options;
using sharpbq.Configuration;
using sharpbq.DataAccess.Clients;

namespace sharpbq.DataAccess;

public class BigQueryClientFactory : IBigQueryClientFactory
{
    private readonly IOptions<BigQueryProjectSettings> _config;

    public BigQueryClientFactory(IOptions<BigQueryProjectSettings> config)
    {
        _config = config;
    }

    public ISharpBQClient Create() => new SharpBQClient(_config);
}