using MongoDB.Bson.Serialization.Attributes;
using System;

namespace YaleRSS.Data
{
    public class LectureEntity
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string LectureId { get; set; }
        public string LectureNumber { get; set; }
        public string Overview { get; set; }

        [BsonIgnore]
        public DateTime DateOfLecture
        {
            get
            {
                // phil181_01_011111
                return DateTime.ParseExact(LectureId.Substring(LectureId.Length - 6, 6),
                      "MMddyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }
    }
}
