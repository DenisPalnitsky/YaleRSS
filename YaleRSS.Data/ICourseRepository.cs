using System.Collections.Generic;

namespace YaleRss.Data
{
    public interface ICourseRepository
    {
        List<CourseEntity> GetAllCourses();
        CourseEntity GetCourse(string courseId);


        
    }
}