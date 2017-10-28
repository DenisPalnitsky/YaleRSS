using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YaleRSS.Data;

namespace YaleRss.Test
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public void TestConnection()
        {
            var repo = new CourseRepository(DbContext.Create());
            var p = repo.Philosophy.Lectures;
            Assert.Greater(p.Count(), 0);
        }

        [Test]
        public void GetFile()
        {
 

        }
    }
}
