using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;

namespace YaleRSS.Controllers
{
    public class RssController : ApiController
    {
        public HttpResponseMessage Get()
        {            
            SyndicationFeed feed = new SyndicationFeed("Feed Title", "Feed Description", new Uri("http://Feed/Alternate/Link"), "FeedID", DateTime.Now);
           
            feed.Contributors.Add(new SyndicationPerson("lene@contoso.com", "Lene Aaling", "http://lene/aaling"));
            feed.Copyright = new TextSyndicationContent("Copyright 2007");
            feed.Description = new TextSyndicationContent("This is a sample feed");

            List<SyndicationItem> items = new List<SyndicationItem>();
            SyndicationItem item = new SyndicationItem()
            {
                Content = SyndicationContent.CreateUrlContent(new Uri("http://feed.thisamericanlife.org/~r/talpodcast/~3/1cae99duMtk/tell-me-im-fat"), "audio/mpeg"),
                Title = new TextSyndicationContent("Lecture1"),
                PublishDate = DateTime.Now.AddDays(-1)
            };

            items.Add(item);
            feed.Items = items;

            feed.Language = "en-us";
            feed.LastUpdatedTime = DateTime.Now;

            var formatter = new Rss20FeedFormatter(feed);
            var xws = new XmlWriterSettings { Encoding = Encoding.UTF8 };


            StringBuilder sb = new StringBuilder() ;
            using (var xmlWriter = XmlWriter.Create(sb, xws))
            {
                formatter.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }
         

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(sb.ToString(),
                Encoding.UTF8,
                "application/rss+xml") };
            return response;
             
        }
    }
}
    