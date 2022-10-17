using Google.Cloud.BigQuery.V2;

namespace sharpbq.DataAccess;

public interface IDataStoreBase
{
    List<T> Query<T>(string queryString, IEnumerable<BigQueryParameter>? parameters = null, CancellationToken token = default);

    Task<List<T>> QueryAsync<T>(string queryString, IEnumerable<BigQueryParameter>? parameters = null, CancellationToken token = default);
}