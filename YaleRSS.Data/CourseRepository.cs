using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using YaleRss.Data.Entities;

namespace YaleRss.Data
{
    public class CourseRepository : RepositoryBase, ICourseRepository
    {
        const string COURSES_COLLECION_NAME = "courses";

        public CourseRepository(IDbContext context) : base(context)
        {
        }

        public List<CourseEntity> GetAllCourses()
        {
            return Context.GetCollection<CourseEntity>(COURSES_COLLECION_NAME).Find(_ => true).ToList();
        }

        public CourseEntity GetCourse(string courseId)
        {           
            var filter = Builders<CourseEntity>.Filter.Eq(c => c.CourseId, courseId);
            return Context.GetCollection<CourseEntity>(COURSES_COLLECION_NAME).Find(c=>c.CourseId == courseId ).Single();           
        }       

    }
 
}
