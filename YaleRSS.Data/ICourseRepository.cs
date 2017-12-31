using System.Collections.Generic;
using YaleRss.Data.Entities;

namespace YaleRss.Data
{
    public interface ICourseRepository
    {
        List<CourseEntity> GetAllCourses();
        CourseEntity GetCourse(string courseId);
    }
}