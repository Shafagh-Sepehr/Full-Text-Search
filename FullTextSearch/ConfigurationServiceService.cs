using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

internal class ConfigurationServiceService : IConfigurationService
{
    private readonly IConfigurationRoot _config;

    public ConfigurationServiceService(IConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        _config = configurationBuilder
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();
    }

    public IConfigurationRoot GetConfig() => _config;
}
