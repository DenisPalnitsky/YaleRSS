using MongoDB.Driver;

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

        internal IMongoCollection<CourseEntity> Cources
        {
            get { return _database.GetCollection<CourseEntity>("courses"); }
        }
      
    }
}
