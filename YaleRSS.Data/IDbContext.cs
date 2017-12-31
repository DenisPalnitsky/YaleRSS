using MongoDB.Driver;
using System.Linq;
using YaleRss.Data.Entities;

namespace YaleRss.Data
{
    public interface IDbContext
    {
        IMongoCollection<T> GetCollection<T>(string collection);
    }
}