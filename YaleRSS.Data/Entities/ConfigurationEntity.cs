using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YaleRss.Data.Entities
{
    public class ConfigurationEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string YaleUrlBase { get; set; }
    }
}
