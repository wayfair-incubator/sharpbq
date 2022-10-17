using Google.Cloud.BigQuery.V2;
using sharpbq.Exceptions;

namespace sharpbq.Extensions;

public static class BigQueryRowExtensions
{
    public static T MapToObject<T>(this BigQueryRow row, List<string> columnNames)
    {
        var rowAsDictionary = columnNames.ToDictionary(columnName => columnName, columnName => row[columnName]);

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(rowAsDictionary);
        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

        if (obj == null)
            throw new ObjectMappingException($"BigQuery result rows do not map to the specified object type {typeof(T)}");
        
        return obj;
    }
}