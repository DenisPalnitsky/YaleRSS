using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace YaleRss.Data
{
    public class CourseRepository : ICourseRepository
    {
        IDbContext _context;

        public CourseRepository(IDbContext context)
        {
            _context = context;
        }

        public List<CourseEntity> GetAllCourses()
        {
            return _context.Cources.ToList();
        }

        public CourseEntity GetCourse(string courseId)
        {           
            var filter = Builders<CourseEntity>.Filter.Eq(c => c.CourseId, courseId);
            return _context.Cources.First(c=>c.CourseId == courseId );           
        }

        public CourseEntity Philosophy
        {
            get
            {
                return _context.Cources.Single(c => c.CourseId == "phil181");
            }
        }

    }
 
}
