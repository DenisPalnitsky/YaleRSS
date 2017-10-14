using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YaleRSS.Data;

namespace YaleRss.Test
{
    [TestFixture]
    public class IntegrationTest
    {
        public void TestConnection()
        {
            var repo = new PodcastRepository(DbContext.Create());
            var p = repo.GetAllPodcasts();
            Assert.Greater(p.Count, 0);
        }
    }
}
