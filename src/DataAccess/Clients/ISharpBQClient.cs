using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;

namespace sharpbq.DataAccess.Clients;

public interface ISharpBQClient
{
    BigQueryResults ExecuteQuery(string sql, IEnumerable<BigQueryParameter> parameters,
        QueryOptions queryOptions = null, GetQueryResultsOptions resultsOptions = null);
    
    Task<BigQueryResults> ExecuteQueryAsync(string sql, IEnumerable<BigQueryParameter> parameters,
        QueryOptions queryOptions = null, GetQueryResultsOptions resultsOptions = null);
    
    BigQueryJob GetJob(JobReference jobReference, GetJobOptions options = null);
    
    Task<BigQueryJob> GetJobAsync(JobReference jobReference, GetJobOptions options = null);
}