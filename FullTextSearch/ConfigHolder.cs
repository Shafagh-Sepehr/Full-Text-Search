using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

public static class ConfigHolder
{
    private static IConfigurationRoot? _config;
    public static IConfigurationRoot Config
    {
        get
        {
            return _config ??= new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
