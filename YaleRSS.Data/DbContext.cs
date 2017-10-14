using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaleRSS.Data
{
    public class DbContext
    {
        IMongoDatabase _database;

        private DbContext() { }

        public static DbContext Create()
        {           
            var client = new MongoClient(@"mongodb://user:911@ds119685.mlab.com:19685/yale_courses");
            var context = new DbContext();
            context._database  = client.GetDatabase("yale_courses");
                        
            return context;            
        }

        public IMongoCollection<PodcastEntity> Philisophy
        {
            get { return _database.GetCollection<PodcastEntity>("lectures");  }
        }
   
    }
}
