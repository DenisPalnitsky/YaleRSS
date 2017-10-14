using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaleRSS.Data
{
    public class PodcastEntity
    {       
        [BsonId]
        public string Id { get; set; }
        public int  order { get; set; } 
        public string name { get; set; }        
    }
}
