﻿using NUnit.Framework;
using System.Linq;
using YaleRss.Data;

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
       
    }
}
