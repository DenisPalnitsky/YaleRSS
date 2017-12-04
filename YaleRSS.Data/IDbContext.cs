using System.Linq;

namespace YaleRss.Data
{
    public interface IDbContext
    {
        IQueryable<CourseEntity> Cources { get; }
    }
}