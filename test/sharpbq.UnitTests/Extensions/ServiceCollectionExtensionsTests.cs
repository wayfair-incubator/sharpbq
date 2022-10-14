using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;
using sharpbq.DataAccess;
using sharpbq.Extensions;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace sharpbq.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBigQueryClient_Types_ShouldMatch()
    {
        // Arrange
        var projectId = "1";
        var credentials = "abcdef";
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "BigQuery:ProjectId", projectId },
                    { "BigQuery:Credentials", credentials }
                })
            .Build();

        // Act
        services.AddBigQueryClient(configuration);

        // Assert
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService<IBigQueryClientFactory>().ShouldBeOfType<BigQueryClientFactory>();
    }

    [Fact]
    public void AddBigQueryClient_BigQueryProjectSettings_ShouldMatch()
    {
        // Arrange
        var projectId = "1";
        var credentials = "abcdef";
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "BigQuery:ProjectId", projectId },
                    { "BigQuery:Credentials", credentials }
                })
            .Build();

        // Act
        services.AddBigQueryClient(configuration);

        // Assert
        var provider = services.BuildServiceProvider();
        var result = provider.GetRequiredService<IOptions<BigQueryProjectSettings>>().Value;

        // Assert
        result.ProjectId.ShouldBe(projectId);
        result.Credentials.ShouldBe(credentials);
    }
}

