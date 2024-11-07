using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch.Tests.ConfigurationService;

public class ConfigurationServiceTests
{
    [Fact]
    public void Constructor_WhenDependenciesAreNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IConfigurationBuilder configurationBuilder = null!;

        // Act & Assert
        Action act = () => new FullTextSearch.ConfigurationService.Services.ConfigurationService(configurationBuilder);
        act.Should().Throw<ArgumentNullException>();
    }
}
