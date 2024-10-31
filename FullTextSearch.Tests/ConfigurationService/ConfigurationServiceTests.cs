using FluentAssertions;
using FullTextSearch.ConfigurationService.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Tests.ConfigurationService;

public class ConfigurationServiceTests
{
    private readonly IConfigurationService? _configurationService = ServiceCollection.ServiceProvider.GetService<IConfigurationService>();
    [Fact]
    public void NotNull()
    {
        _configurationService.Should().NotBeNull();
    }
}
