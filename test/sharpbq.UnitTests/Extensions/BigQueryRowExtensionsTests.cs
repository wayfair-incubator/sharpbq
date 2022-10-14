using Google.Cloud.BigQuery.V2;
using Google.Apis.Bigquery.v2.Data;
using sharpbq.Extensions;
using Shouldly;
using System.Collections.Generic;
using Xunit;
using System;

namespace sharpbq.UnitTests.Extensions;

public class BigQueryRowExtensionsTests
{
    [Fact]
    public void MapToObject_ShouldMatch()
    {
        // Arrange
        var columnNames = new List<string> { "Id", "Name" };
        var tableRow = new TableRow
        {
            ETag = "myTable",
            F = new List<TableCell>()
            {
                new TableCell { ETag = "Id", V = "12345" },
                new TableCell { ETag = "Name", V = "MyName" }
            }
        };
        var tableSchema = new TableSchema
        {
            ETag = "myTableSchema",
            Fields = new List<TableFieldSchema>()
            {
                new TableFieldSchema { ETag = "Id", Name = "Id", Type = "STRING" },
                new TableFieldSchema { ETag = "Name", Name = "Name", Type = "STRING" },
            }
        };
        BigQueryRow row = new(tableRow, tableSchema);
        var expected = new SampleRecord("12345", "MyName");

        // Act
        var result = row.MapToObject<SampleRecord>(columnNames);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void MapToObject_EmptyColumns_ShouldReturnDefault()
    {
        BigQueryRow row = new(new TableRow(), new TableSchema());

        // Act
        var result = row.MapToObject<SampleRecord>(new List<string>());

        // Assert
        result.ShouldBeAssignableTo(typeof(SampleRecord));
        result.Id.ShouldBeNull();
        result.Name.ShouldBeNull();
    }
}

public record SampleRecord(string Id, string Name);
