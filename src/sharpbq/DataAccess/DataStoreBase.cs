using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;

namespace sharpbq.DataAccess;

public abstract class BigQueryDataStoreBase
{
    protected abstract int TimeoutInMinutes { get; }

    private readonly BigQueryProjectSettings _config;

    protected BigQueryDataStoreBase(IOptions<BigQueryProjectSettings> config)
    {
        _config = config.Value;
    }

    protected List<T> Query<T>(string queryString, BigQueryClient client = null)
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
        if (results.TotalRows != null && results.TotalRows > 0)
        {
            var columnNames = Enumerable.ToList<string>(results.Schema.Fields.Select(f => f.Name));
            resultsList = Enumerable.ToList<T>(results.Select(r => MapRowToObject<T>(r, columnNames)));
        }

        return resultsList;
    }

    protected async Task<List<T>> QueryAsync<T>(string queryString, List<BigQueryParameter> parameters = null,
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
        if (results.TotalRows != null && results.TotalRows > 0)
        {
            var columnNames = Enumerable.ToList<string>(results.Schema.Fields.Select(f => f.Name));
            resultsList = Enumerable.ToList<T>(results.Select(r => MapRowToObject<T>(r, columnNames)));
        }

        return resultsList;
    }

    private T MapRowToObject<T>(BigQueryRow row, List<string> columnNames)
    {
        var rowAsDictionary = new Dictionary<string, object>();
        foreach (var columnName in columnNames)
        {
            rowAsDictionary.Add(columnName, row[columnName]);
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(rowAsDictionary);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }
}