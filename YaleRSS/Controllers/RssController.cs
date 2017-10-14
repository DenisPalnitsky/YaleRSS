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

        PodcastRepository _repo = new PodcastRepository(DbContext.Create());


        public HttpResponseMessage Get()
        {            
           

            SyndicationFeed feed = new SyndicationFeed("Yale lectures", "Yale lectures", new Uri("http://Feed/Alternate/Link"), "FeedID", DateTime.Now);
                       
            feed.Copyright = new TextSyndicationContent("No Copyright");
            feed.Description = new TextSyndicationContent("This is a sample feed");

            List<SyndicationItem> items = new List<SyndicationItem>();
            var lectures = _repo.GetAllPodcasts();

            foreach (var lecture in lectures)
            {
                SyndicationItem item = new SyndicationItem()
                {
                    Content = SyndicationContent.CreateUrlContent(
                        new Uri("http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3"), "audio/mpeg"),

                    Title = new TextSyndicationContent(lecture.name),                     
                    PublishDate = DateTime.Now.AddDays(-1)
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
    