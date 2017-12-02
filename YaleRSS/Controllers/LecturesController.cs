using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using YaleRss.Data;

namespace YaleRss.Controllers
{
    [Route("api/[controller]", Name = RouteNames.UserProfile)]
    public class LecturesController : Controller
    {
        CourseRepository _repo = new CourseRepository(DbContext.Create());

        [HttpGet("{id}")]
        public HttpResponseMessage GetLectures(string id)
        {            
            Trace.WriteLine($"Downloading lecture { id }");
            var course = _repo.Philosophy;
            var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));
                                
            Trace.WriteLine("Requesting file from storage");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(String.Format(course.AlternativeAudioUrlPattern, lecture.LectureId));
            
            Trace.WriteLine("Returning response");
            return response;
        }

        
        
    }
}
