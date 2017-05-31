using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;

namespace YaleRSS.Controllers
{
    public class RssController : ApiController
    {
        public RssActionResult Get()
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

            return new RssActionResult() { Feed = feed }; 
        }
    }
}
    