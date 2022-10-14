using Google.Cloud.BigQuery.V2;

namespace sharpbq.DataAccess;

public interface IDataStoreBase
{
    List<T> Query<T>(string queryString, BigQueryClient client = null);

    Task<List<T>> QueryAsync<T>(string queryString, List<BigQueryParameter> parameters = null, BigQueryClient client = null);
}