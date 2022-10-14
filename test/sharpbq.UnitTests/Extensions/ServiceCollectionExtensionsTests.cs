using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using sharpbq.Configuration;
using sharpbq.DataAccess;
using sharpbq.DataAccess.Clients;
using sharpbq.Extensions;
using Shouldly;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace sharpbq.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBigQueryClient_Types_ShouldMatch()
    {
        // Arrange
        var projectId = "1";
        var credentials = GetCredentials();
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
        provider.GetRequiredService<IDataStoreBase>().ShouldBeOfType<DataStoreBase>();
        provider.GetRequiredService<ISharpBQClient>().ShouldBeOfType<SharpBQClient>();
    }

    [Fact]
    public void AddBigQueryClient_BigQueryProjectSettings_ShouldMatch()
    {
        // Arrange
        var projectId = "1";
        var credentials = GetCredentials();
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

    private string GetCredentials()
        => JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "type", "authorized_user" },
            { "project_id", "random" },
            { "quota_project_id", "random" },
            { "client_id", "random" },
            { "client_secret", "random" },
            { "client_email", "random" },
            { "private_key", "random" },
            { "private_key_id", "random" },
            { "refresh_token", "random" },
            { "audience", "random" },
            { "subject_token_type", "random" },
            { "token_url", "random" },
            { "service_account_impersonation_url", "random" },
            { "workforce_pool_user_project", "random" },
            { "credential_source", new Dictionary<string, object> {
                    { "environment_id", "random" },
                    { "region_url", "random" },
                    { "url", "random" },
                    { "regional_cred_verification_url", "random" },
                    { "imdsv2_session_token_url", "random" },
                    { "headers", new Dictionary<string, string>() },
                    { "file", "random" },
                    { "format", new { type = "", subject_token_field_name = "" } }
                }
            }
        });
}

