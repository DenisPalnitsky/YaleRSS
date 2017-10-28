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
using System.Xml.Linq;
using YaleRSS.Data;

namespace YaleRSS.Controllers
{
    public class RssController : ApiController
    {

        CourseRepository _repo = new CourseRepository(DbContext.Create());

        readonly DateTime _startDate = new DateTime(2011, 1, 18);


        public HttpResponseMessage Get()
        {             
            SyndicationFeed feed = new SyndicationFeed("Yale lectures", "Yale lectures",
                new Uri(this.ActionContext.Request.RequestUri.AbsoluteUri),  
                this.ActionContext.Request.RequestUri.AbsoluteUri , 
                DateTime.Now);
            feed.Id = this.ActionContext.Request.RequestUri.AbsoluteUri;


            feed.Copyright = new TextSyndicationContent("Most of the lectures and course material within Open Yale Courses are licensed under a Creative Commons Attribution-Noncommercial-Share Alike 3.0 license. ");
            feed.Description = new TextSyndicationContent("This is a sample feed");

            List<SyndicationItem> items = new List<SyndicationItem>();
            var course = _repo.Philosophy;
           

            foreach (var lecture in course.Lectures)
            {
                SyndicationItem item = new SyndicationItem(
                    lecture.Name,
                    SyndicationContent.CreateUrlContent(GetAudioUri( lecture), "audio/mpeg"), 
                    GetAudioUri( lecture),
                    GetAudioUri( lecture).ToString(), 
                    DateTimeOffset.Now.AddDays(-1));

                item.ElementExtensions.Add(
                            new XElement("enclosure",
                                new XAttribute("type", "audio/mpeg"),
                                new XAttribute("url", GetAudioUri( lecture).ToString())
                            ).CreateReader()
                        );

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

        private Uri GetAudioUri(LectureEntity lecture)
        {
            
            return new Uri(this.Url.Link("DefaultApi", new {  Controller = "Lectures",  id = lecture.LectureId }));           
            //return Url.Link("//", lecture.LectureId) );
        }
    }
}
    