using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaleRSS.Data
{
    public class LectureEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int  Order { get; set; } 
        public string Name { get; set; }
        public string LectureId { get; set; }
    }
}
