using MongoDB.Driver;
using System.Linq;

namespace YaleRss.Data
{
    public interface IDbContext
    {
        IQueryable<CourseEntity> Courses { get; }       
    }
}