using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace YaleRss.Data
{
    [BsonIgnoreExtraElements]
    public class CourseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string CourseIdReadeble { get; set; }
        public string Name { get; set; }
        public string YaleUrlPattern { get; set; }
        public string InternalUrlPattern { get; set; }
        public string DepartmentLink { get; set; }
        public string Department { get; set; } 
        public string CourseLink { get; set; }
        public List<LectureEntity> Lectures { get; set; } = new List<LectureEntity>();
    }
}
