using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaleRSS.Data
{
    public class PodcastRepository
    {
        DbContext _context;

        public PodcastRepository(DbContext context)
        {
            _context = context;
        }

        public List<PodcastEntity> GetAllPodcasts()
        {
            return _context.Philisophy.Find(_ => true).ToList();
        }

        public PodcastEntity GetEntity(string id)
        {
            var filter = Builders<PodcastEntity>.Filter.Eq("Id", id);
            return _context.Philisophy
                                 .Find(filter)
                                 .FirstOrDefault();
        }
    }
}
