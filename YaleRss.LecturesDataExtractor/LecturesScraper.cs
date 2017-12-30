using System;
using System.Collections.Generic;
using System.Text;
using YaleRss.Data;
using System.Linq;
using MongoDB.Driver;

namespace YaleRss.LecturesDataExtractor
{
    public class LecturesScraper
    {
        private IMongoDatabase _database;



        public void ScrapeLectures()
        {
            var client = new MongoClient(@"mongodb://user:@ds119685.mlab.com:19685/yale_courses");
            _database = client.GetDatabase("yale_courses");

            foreach (var course in _database.GetCollection<CourseEntity>("courses").AsQueryable())
            {
                if (course.Lectures.Count() == 0)
                {
                    var lectureUrls = new List<string>();
                    for (int i = 1; i < 30; i++)
                    {
                        lectureUrls.Add($"https://oyc.yale.edu/{ course.DepartmentLink }/{course.CourseIdReadeble.Replace(' ', '-')}/lecture-{i}");
                    }

                    var s = Program.GetAllLinksToMedia(lectureUrls, Program.StringBetweenTwo(course.YaleUrlPattern, "/courses/", "/mp3/{0}.mp3"));

                    foreach (var l in s)
                    {
                        AddLecture(course, l);
                    }
                }
            }
        }

        public void AddLecture(CourseEntity course, LectureEntity lecture)
        {
            var collection = _database.GetCollection<CourseEntity>("courses");
            UpdateDefinition<CourseEntity> update = Builders<CourseEntity>.Update.Push(x => x.Lectures, lecture);
            var filter = Builders<CourseEntity>.Filter.Eq(x => x.Id, course.Id);
            collection.UpdateOne(filter, update);
        }
    }
}
