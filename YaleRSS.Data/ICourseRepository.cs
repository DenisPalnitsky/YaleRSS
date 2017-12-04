using System.Collections.Generic;

namespace YaleRss.Data
{
    public interface ICourseRepository
    {
        CourseEntity Philosophy { get; }

        List<CourseEntity> GetAllCourses();
        CourseEntity GetCourse(string courseId);       
    }
}