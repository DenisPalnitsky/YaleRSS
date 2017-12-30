﻿using MongoDB.Driver;
using System.Linq;

namespace YaleRss.Data
{
    public class DbContext : IDbContext
    {
        private IMongoDatabase _database;

        public DbContext() {

            var client = new MongoClient(@"mongodb://reader:911@ds119685.mlab.com:19685/yale_courses");
            _database = client.GetDatabase("yale_courses");
        }

        // This property may require to return IMongoCollection if IQueryable will misbehave
        public IQueryable<CourseEntity> Courses
        {            
            get { return _database.GetCollection<CourseEntity>("courses").AsQueryable(); }
        }

        
      
    }
}
