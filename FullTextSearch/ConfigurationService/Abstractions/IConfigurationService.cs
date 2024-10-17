using Microsoft.Extensions.Configuration;

namespace FullTextSearch.ConfigurationService.Abstractions;

public interface IConfigurationService
{
    IConfigurationRoot GetConfig();
}
