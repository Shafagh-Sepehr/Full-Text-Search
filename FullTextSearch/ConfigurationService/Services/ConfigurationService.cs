using FullTextSearch.ConfigurationService.Abstractions;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch.ConfigurationService.Services;

internal class ConfigurationService : IConfigurationService
{
    private readonly IConfigurationRoot _config;
    
    public ConfigurationService(IConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        
        _config = configurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("inverted_index_appsettings.json", false, true)
            .Build();
    }
    
    public IConfigurationRoot GetConfig() => _config;
}
