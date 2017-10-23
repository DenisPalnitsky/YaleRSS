using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YaleRSS.Data
{


    public class CourseRepository
    {
        DbContext _context;

        public CourseRepository(DbContext context)
        {
            _context = context;
        }

        public List<CourseEntity> GetAllCourses()
        {
            return _context.Cources.Find(_ => true).ToList();
        }

        public CourseEntity Philosophy
        {
             get {
                var filter = Builders<CourseEntity>.Filter.Eq(c=> c.CourseId, "phil181");
                return _context.Cources
                                    .Find(filter).Single();
                }
        }

        public CourseEntity GetEntity(string id)
        {
            var filter = Builders<CourseEntity>.Filter.Eq("Id", id);
            return _context.Cources
                                .Find( filter )
                                 .FirstOrDefault();
        }
    }
 
}
