using Google.Cloud.BigQuery.V2;

namespace sharpbq.DataAccess;

public interface IBigQueryClientFactory
{
    ISharpBQClient Create();
}