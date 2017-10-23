using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using YaleRSS.Data;

namespace YaleRSS.Controllers
{
    public class RssController : ApiController
    {

        CourseRepository _repo = new CourseRepository(DbContext.Create());

        readonly DateTime _startDate = new DateTime(2011, 1, 18);


        public HttpResponseMessage Get()
        {            
           

            SyndicationFeed feed = new SyndicationFeed("Yale lectures", "Yale lectures", new Uri(this.ActionContext.Request.RequestUri.AbsoluteUri),  this.ActionContext.Request.RequestUri.AbsoluteUri , DateTime.Now);
                       
            feed.Copyright = new TextSyndicationContent("No Copyright");
            feed.Description = new TextSyndicationContent("This is a sample feed");

            List<SyndicationItem> items = new List<SyndicationItem>();
            var course = _repo.Philosophy;
           

            foreach (var lecture in course.Lectures)
            {
                SyndicationItem item = new SyndicationItem()
                {
                    Content = SyndicationContent.CreateUrlContent(
                        new Uri(String.Format(course.AudioUrlPattern, lecture.Order)), "audio/mpeg"),

                    Title = new TextSyndicationContent(lecture.Name),
                    PublishDate = _startDate.AddDays(lecture.Order)
                };
                items.Add(item);
            }

            feed.Items = items;

            feed.Language = "en-us";
            feed.LastUpdatedTime = DateTime.Now;

            var formatter = new Rss20FeedFormatter(feed);
            
            var xws = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };


            using (MemoryStream ms = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(ms, xws))
                {
                    formatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(Encoding.UTF8.GetString(ms.GetBuffer()),
                    Encoding.UTF8,
                    "application/rss+xml")
                };

                return response;
            }   
        }
       
    }
}
    