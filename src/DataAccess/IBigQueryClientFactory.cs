namespace sharpbq.DataAccess;

public interface IBigQueryClientFactory
{
    ISharpBQClient Create();
}