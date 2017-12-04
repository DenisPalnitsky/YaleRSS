using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using YaleRss.Data;

namespace YaleRss.Controllers
{

    [Route("api/[controller]", Name = RouteNames.UserProfile)]
    public class RssController : Controller
    {

        ICourseRepository _repo;

        readonly DateTime _startDate = new DateTime(2011, 1, 18);

        public RssController (ICourseRepository courseRepository)
        {
            _repo = courseRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {                       
            List<SyndicationItem> items = new List<SyndicationItem>();
            var course = _repo.Philosophy;
           
            foreach (var lecture in course.Lectures)
            {
                SyndicationItem item = new SyndicationItem()
                {
                    Id = $"{lecture.LectureNumber}.{lecture.Name}",
                    Published = lecture.DateOfLecture, 
                    Title = lecture.Name,
                    Description = lecture.Overview,
                };

                item.AddLink ( new SyndicationLink(GetAudioUri(lecture)) { MediaType = "audio/mpeg" } );
                item.AddCategory(new SyndicationCategory("Education"));
                item.LastUpdated = lecture.DateOfLecture;
          
                items.Add(item);
            }

   
            var formatter = new RssFormatter();            
            var xws = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8, Async = true };            

            using (MemoryStream ms = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(ms, xws))
                {
                    RssFeedWriter feedWriter = new RssFeedWriter(xmlWriter);
                    feedWriter.WriteDescription("Lectures from Open Yale Courses http://oyc.yale.edu/ ");
                    feedWriter.WriteCopyright("Most of the lectures and course material within Open Yale Courses are licensed under a Creative Commons Attribution-Noncommercial-Share Alike 3.0 license. ");
                    feedWriter.WriteDescription("Lectures from Open Yale Courses http://oyc.yale.edu/ ");
                    feedWriter.WriteTitle("Yale Open lectures");
                    feedWriter.WriteLastBuildDate(DateTime.Now);
                    
                    feedWriter.WriteLanguage(CultureInfo.GetCultureInfo("en-us"));

                    foreach (var item in items) {
                       
                        var content = new SyndicationContent(formatter.CreateContent(item));

                        var enclosureItem = new SyndicationContent("enclosure", "Custom Value");
                        enclosureItem.AddAttribute(new SyndicationAttribute("type", "audio/mpeg"));
                        enclosureItem.AddAttribute(new SyndicationAttribute("url", item.Links.First().Uri.ToString() ));

                        // Add custom fields/attributes
                        content.AddField(enclosureItem);

                        feedWriter.Write(content);
                    }                                         
                }

                var response = new ContentResult()
                {
                    Content = Encoding.UTF8.GetString(ms.ToArray()),
                    StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status200OK,
                    ContentType = "application/rss+xml"
                };

                return response;
            }   
        }

        private Uri GetAudioUri(LectureEntity lecture)
        {
            var url = this.Url.Link(RouteNames.Lectures, new { Controller = "Lectures", id = lecture.LectureId });
            return new Uri(url);           
        }
    }

   
}
    