using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using YaleRss.Data;

namespace YaleRss.Controllers
{
    //[Route("api/[controller]", Name = RouteNames.UserProfile)]
    public class LecturesController : Controller
    {
        CourseRepository _repo = new CourseRepository(DbContext.Create());

        [HttpGet("api/lectures/{id}", Name=RouteNames.Lectures )]
        public IActionResult GetLectures(string id)
        {            
            Trace.WriteLine($"Downloading lecture { id }");
            var course = _repo.Philosophy;
            var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));

            var yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AudioUrlPattern, lecture.LectureId));
           
            if (yaleResponse.StatusCode == HttpStatusCode.OK)
            {
                Trace.WriteLine("Attempt to get stream from Yale succeeded");                  
            }
            else
            {
                Trace.WriteLine("Attempt to get stream from Yale failed. Sourcing from storage");
                yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AlternativeAudioUrlPattern, lecture.LectureId));
            }

            FileStreamResult response = new FileStreamResult(yaleResponse.GetResponseStream(),
                 yaleResponse.ContentType);
            

            Trace.WriteLine("Setting headers");
            
            response.FileDownloadName = lecture.LectureId + ".mp3";
          
            Trace.WriteLine("Returning response");
            return response;
        }

        
        
    }
}
