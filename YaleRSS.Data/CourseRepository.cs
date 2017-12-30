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
            return _context.Courses.ToList();
        }

        public CourseEntity GetCourse(string courseId)
        {           
            var filter = Builders<CourseEntity>.Filter.Eq(c => c.CourseId, courseId);
            return _context.Courses.First(c=>c.CourseId == courseId );           
        }       

    }
 
}
