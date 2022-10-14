using sharpbq.DataAccess.Clients;

namespace sharpbq.DataAccess;

public interface IBigQueryClientFactory
{
    ISharpBQClient Create();
}