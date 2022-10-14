using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;
using sharpbq.Extensions;

namespace sharpbq.DataAccess;

public abstract class DataStoreBase : IDataStoreBase
{
    protected abstract int TimeoutInMinutes { get; }

    private readonly BigQueryProjectSettings _config;

    protected DataStoreBase(IOptions<BigQueryProjectSettings> config)
    {
        _config = config.Value;
    }

    public List<T> Query<T>(string queryString, BigQueryClient client = null)
    {
        client ??= BigQueryClient.Create(_config.ProjectId, GoogleCredential.FromJson(_config.Credentials));

        var results = client.ExecuteQuery(queryString, parameters: null, new QueryOptions { UseQueryCache = false },
            new GetQueryResultsOptions { Timeout = TimeSpan.FromMinutes(TimeoutInMinutes) });

        var job = client.GetJob(results.JobReference);
        if (job.Status.ErrorResult != null)
        {
            throw new Exception(job.Status.ErrorResult.Message);
        }

        var resultsList = new List<T>();
        if (results.TotalRows is > 0)
        {
            var columnNames = results.Schema.Fields.Select(f => f.Name).ToList();
            resultsList = results.Select(r => r.MapToObject<T>(columnNames)).ToList();
        }

        return resultsList;
    }

    public async Task<List<T>> QueryAsync<T>(string queryString, List<BigQueryParameter> parameters = null,
        BigQueryClient client = null)
    {
        client ??= await BigQueryClient.CreateAsync(_config.ProjectId, GoogleCredential.FromJson(_config.Credentials));

        var results = await client.ExecuteQueryAsync(queryString, parameters: parameters,
            new QueryOptions { UseQueryCache = false },
            new GetQueryResultsOptions { Timeout = TimeSpan.FromMinutes(TimeoutInMinutes) });

        var job = await client.GetJobAsync(results.JobReference);
        if (job.Status.ErrorResult != null)
        {
            throw new Exception(job.Status.ErrorResult.Message);
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