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
        public string Name { get; set; }
        public string AudioUrlPattern { get; set; }
        public string AlternativeAudioUrlPattern { get; set; }
        public IEnumerable<LectureEntity> Lectures { get; set; }
    }
}
