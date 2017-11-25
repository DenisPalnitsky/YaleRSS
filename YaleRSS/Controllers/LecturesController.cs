using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using YaleRSS.Data;

namespace YaleRSS.Controllers
{
    public class LecturesController : ApiController
    {
        CourseRepository _repo = new CourseRepository(DbContext.Create());

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
