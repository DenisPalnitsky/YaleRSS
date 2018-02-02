using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using YaleRss.Data;
using YaleRss.Data.Entities;

namespace YaleRss.Controllers
{
    public class RssController : Controller
    {
        ICourseRepository _repo;

        readonly DateTime _startDate = new DateTime(2011, 1, 18);

        public RssController (ICourseRepository courseRepository)
        {
            _repo = courseRepository;
        }

        [HttpGet("rss/list")]
        public IActionResult Get()
        {
            var result = _repo.GetAllCourses().Select(c => new
            {
                c.Name,
                CourseLink = "https://oyc.yale.edu/" + c.CourseLink,
                c.Department,
                c.DepartmentLink,
                c.CourseId,
                c.CourseIdReadeble,
                c.YaleUrlPattern,
                c.InternalUrlPattern,
                Link = this.Url.Link(RouteNames.Courses, new { Controller = "RssController", id = c.CourseId }),
                NubmerOfLectures = c.Lectures.Count(),
                IsAvailable = c.Lectures.Count() != 0,
                c.IsRecommended
            });

            return new JsonResult(result.OrderByDescending(c=>c.IsAvailable), new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }
    

        [HttpGet("rss/{id}", Name = RouteNames.Courses)]
        public IActionResult Get(string id)
        {
            var course = _repo.GetCourse(id.ToLower());

            List<SyndicationItem> items = BuildRssItems(course);

            var rssStream = BuildRssStream(items, course.Name);

            return PrepareActionResult(rssStream);
        }

        private static IActionResult PrepareActionResult(byte[] rssStream)
        {
            return new ContentResult()
            {
                Content = Encoding.UTF8.GetString(rssStream),
                StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status200OK,
                ContentType = "text/xml"
            };
        }

        [HttpGet("rss/all")]
        public IActionResult GetAllLectures()
        {
            var courses = _repo.GetAllCourses();
            var items = new List<SyndicationItem>();
            foreach (var course in courses.OrderByDescending(c=> c.IsRecommended )) 
            {
                items.AddRange(BuildRssItems(course)); 
            }

            var rssStream = BuildRssStream(items, "All Lectures");

            return PrepareActionResult(rssStream);
        }


        private List<SyndicationItem> BuildRssItems(CourseEntity course)
        {
            var lectures = course.Lectures;

            if (lectures == null)
                lectures = new List<LectureEntity>();

            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (var lecture in lectures)
            {
                SyndicationItem item = CreateSyndicationItem(course.CourseId, lecture);
                items.Add(item);
            }

            return items;
        }

        private byte[] BuildRssStream(List<SyndicationItem> items, string feedTitle)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new RssFormatter();
                var xws = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8, Async = true };
                using (var xmlWriter = XmlWriter.Create(ms, xws))
                {
                    RssFeedWriter feedWriter = new RssFeedWriter(xmlWriter);
                    feedWriter.WriteDescription("Lectures from Open Yale Courses. More info on http://oyc.yale.edu/ ");
                    feedWriter.WriteCopyright("Most of the lectures and course material within Open Yale Courses are licensed under a Creative Commons Attribution-Noncommercial-Share Alike 3.0 license. ");
                    feedWriter.WriteTitle($"Yale Open lectures. {feedTitle}");
                    feedWriter.WriteLastBuildDate(DateTime.Now);

                    var sendycationImage = new SyndicationImage(this.Url.AbsoluteContent(@"~/images/square_logo_large.jpg"))
                    {
                        Title = "YaleLogo",
                        Link = new SyndicationLink(this.Url.AbsoluteContent(@"~/images/square_logo_large.jpg"))
                    };


                    feedWriter.Write(sendycationImage);


                    feedWriter.WriteLanguage(CultureInfo.GetCultureInfo("en-us"));

                    foreach (var item in items)
                    {
                        var content = new SyndicationContent(formatter.CreateContent(item));

                        var enclosureItem = new SyndicationContent("enclosure", "Custom Value");
                        enclosureItem.AddAttribute(new SyndicationAttribute("type", "audio/mpeg"));
                        enclosureItem.AddAttribute(new SyndicationAttribute("url", item.Links.First().Uri.ToString()));

                        // Add custom fields/attributes
                        content.AddField(enclosureItem);

                        feedWriter.Write(content);
                    }
                }
                return ms.ToArray();
            }
        }

        private SyndicationItem CreateSyndicationItem(string courseId, LectureEntity lecture)
        {
            SyndicationItem item = new SyndicationItem()
            {
                Id = $"{lecture.LectureNumber}.{lecture.Name}",
                Published = lecture.DateOfLecture,
                Title = WebUtility.HtmlDecode( lecture.Name),
                Description = WebUtility.HtmlDecode(lecture.Overview),
            };

            item.AddLink(new SyndicationLink(GetAudioUri(courseId, lecture)) { MediaType = "audio/mpeg" });
            item.AddCategory(new SyndicationCategory("Education"));
            item.LastUpdated = lecture.DateOfLecture;
            return item;
        }

        private Uri GetAudioUri(string courseId, LectureEntity lecture)
        {
            var url = this.Url.Link(RouteNames.Lectures, new { Controller = nameof(LecturesController), courseId, id = lecture.LectureId });
            return new Uri(url);           
        }
    }

   
}
    