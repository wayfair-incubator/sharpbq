using Google.Cloud.BigQuery.V2;
using sharpbq.DataAccess.Clients;
using sharpbq.Exceptions;
using sharpbq.Extensions;

namespace sharpbq.DataAccess;

public class DataStoreBase : IDataStoreBase
{
    private const int TimeoutInMinutes = 5;

    private readonly ISharpBQClient _client;

    public DataStoreBase(IBigQueryClientFactory factory)
    {
        _client = factory.Create();
    }

    public List<T> Query<T>(string queryString, IEnumerable<BigQueryParameter>? parameters = null,
        CancellationToken token = default)
    {
        var results = _client.ExecuteQuery(queryString, parameters, new QueryOptions { UseQueryCache = false },
            new GetQueryResultsOptions { Timeout = TimeSpan.FromMinutes(TimeoutInMinutes) });

        var job = _client.GetJob(results.JobReference);
        if (job.Status.ErrorResult != null)
        {
            throw new ErrorResultException(job.Status.ErrorResult.Message);
        }

        var resultsList = new List<T>();
        if (results.TotalRows is > 0)
        {
            var columnNames = results.Schema.Fields.Select(f => f.Name).ToList();
            resultsList = results.Select(r => r.MapToObject<T>(columnNames)).ToList();
        }

        return resultsList;
    }

    public async Task<List<T>> QueryAsync<T>(string queryString, IEnumerable<BigQueryParameter>? parameters = null,
        CancellationToken token = default)
    {
        var results = await _client.ExecuteQueryAsync(queryString, parameters,
            new QueryOptions { UseQueryCache = false },
            new GetQueryResultsOptions { Timeout = TimeSpan.FromMinutes(TimeoutInMinutes) },
            token);

        var job = await _client.GetJobAsync(results.JobReference);
        if (job.Status.ErrorResult != null)
        {
            throw new ErrorResultException(job.Status.ErrorResult.Message);
        }

        var resultsList = new List<T>();
        if (results.TotalRows is > 0)
        {
            var columnNames = results.Schema.Fields.Select(f => f.Name).ToList();
            resultsList = results.Select(r => r.MapToObject<T>(columnNames)).ToList();
        }

        return resultsList;
    }
}