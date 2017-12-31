using YaleRss.Data.Entities;

namespace YaleRss.Data
{
    public interface IConfigurationRepository
    {
        ConfigurationEntity GetConfiguration();
    }
}