using Google.Apis.Auth.OAuth2;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;

namespace sharpbq.DataAccess.Clients;

public class SharpBQClient : ISharpBQClient
{
    private readonly BigQueryClient _client;
    
    public SharpBQClient(IOptions<BigQueryProjectSettings> config)
    {
        _client = BigQueryClient.Create(config.Value.ProjectId, GoogleCredential.FromJson(config.Value.Credentials));
    }

    public BigQueryResults ExecuteQuery(string sql, IEnumerable<BigQueryParameter> parameters,
        QueryOptions queryOptions = null,
        GetQueryResultsOptions resultsOptions = null) =>
        _client.ExecuteQuery(sql, parameters, queryOptions, resultsOptions);

    public async Task<BigQueryResults> ExecuteQueryAsync(string sql, IEnumerable<BigQueryParameter> parameters,
        QueryOptions queryOptions = null,
        GetQueryResultsOptions resultsOptions = null) =>
        await _client.ExecuteQueryAsync(sql, parameters, queryOptions, resultsOptions);

    public BigQueryJob GetJob(JobReference jobReference, GetJobOptions options = null) =>
        _client.GetJob(jobReference, options);
    
    public async Task<BigQueryJob> GetJobAsync(JobReference jobReference, GetJobOptions options = null) =>
        await _client.GetJobAsync(jobReference, options);
}