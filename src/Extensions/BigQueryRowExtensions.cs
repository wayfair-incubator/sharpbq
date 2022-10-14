using Google.Cloud.BigQuery.V2;

namespace sharpbq.Extensions;

public static class BigQueryRowExtensions
{
    public static T MapToObject<T>(this BigQueryRow row, List<string> columnNames)
    {
        var rowAsDictionary = new Dictionary<string, object>();
        foreach (var columnName in columnNames)
        {
            rowAsDictionary.Add(columnName, row[columnName]);
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(rowAsDictionary);
        T? obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

        if (obj == null)
            throw new Exception($"BigQuery result rows do not map to the specified object type {typeof(T)}");
        
        return obj;
    }
}