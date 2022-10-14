using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace sharpbq.UnitTests.Configuration;

public class BigQueryProjectSettingsTest
{
    [Fact]
    public void DefaultSettings_ShouldMatch()
    {
        // Arrange
        BigQueryProjectSettings Subject = new();

        // Act

        // Assert
        Subject.ProjectId.ShouldBeNull();
        Subject.Credentials.ShouldBeNull();
    }

    [Fact]
    public void IOptions_ConfigSettings_ShouldMatch()
    {
        // Arrange
        var projectId = "99999999999";
        var credentials = "khsgfkjhfksgskdhgdkjfhdfgkdh";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "BigQuery:ProjectId", "99999999999" },
                    { "BigQuery:Credentials", "khsgfkjhfksgskdhgdkjfhdfgkdh" }
                })
            .Build();
        var services = new ServiceCollection();
        services.Configure<BigQueryProjectSettings>(options => configuration.GetSection("BigQuery").Bind(options));
        var provider = services.BuildServiceProvider();

        // Act
        var result = provider.GetRequiredService<IOptions<BigQueryProjectSettings>>().Value;

        // Assert
        result.ProjectId.ShouldBe(projectId);
        result.Credentials.ShouldBe(credentials);
    }
}

