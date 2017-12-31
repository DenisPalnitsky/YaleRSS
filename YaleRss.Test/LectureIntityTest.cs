using NUnit.Framework;
using System;
using YaleRss.Data;
using YaleRss.Data.Entities;

namespace YaleRss.Test
{
    [TestFixture]
    public class LectureIntityTest
    {
        [Test]
        public void DateOfLecture_returns_date()
        {
            LectureEntity lecture = new LectureEntity() { LectureId = "phil181_01_010511" };
            Assert.AreEqual(new DateTime(2011, 1, 5), lecture.DateOfLecture);
        }
    }
}
