using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

public interface IConfigurationService
{
    IConfigurationRoot GetConfig();
}
