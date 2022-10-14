using sharpbq.Configuration;
using Shouldly;
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
}

