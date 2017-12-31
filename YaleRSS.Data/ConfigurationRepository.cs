using MongoDB.Driver;
using YaleRss.Data.Entities;

namespace YaleRss.Data
{
    public class ConfigurationRepository : RepositoryBase, IConfigurationRepository
    {
        public ConfigurationRepository(IDbContext context) : base(context)
        {
        }

        public ConfigurationEntity GetConfiguration()
        {
            return Context.GetCollection<ConfigurationEntity>("configuration").Find( _ => true).Single();
        }
    }
}
