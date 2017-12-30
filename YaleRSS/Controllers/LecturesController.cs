using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using YaleRss.Data;
using YaleRSS.Data.WebData;

namespace YaleRss.Controllers
{
    //[Route("api/[controller]", Name = RouteNames.UserProfile)]
    public class LecturesController : Controller
    {
        ICourseRepository _repo;

        public LecturesController (ICourseRepository courseRepository)
        {
            _repo = courseRepository;
        }

        [HttpGet("rss/lectures/{courseId}/{id}.mp3", Name=RouteNames.Lectures )]
        public IActionResult GetLectures(string courseId, string id)
        {            
            Trace.WriteLine($"Downloading lecture { id }");
            var course = _repo.GetCourse(courseId);

            var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));

            if (!String.IsNullOrEmpty(course.InternalUrlPattern))
            {
                Trace.WriteLine("Redirecting to Azure storage");
                return RedirectPreserveMethod(String.Format(course.InternalUrlPattern, lecture.LectureId));
            }
        
            Trace.WriteLine("Attempt to get stream from Yale failed. Sourcing from storage");
            var yaleResponse = YaleSiteRequest.GetFile(String.Format(course.YaleUrlPattern, lecture.LectureId));
       
            if (yaleResponse.ContentLength> 0)
                Response.Headers.ContentLength = yaleResponse.ContentLength;

            FileStreamResult response = new FileStreamResult(yaleResponse.GetResponseStream(),
                 yaleResponse.ContentType);

            Trace.WriteLine("Setting headers");
                                
            response.FileDownloadName = lecture.LectureId + ".mp3";
          
            Trace.WriteLine("Returning response");
            return response;
        }

        
        
    }
}
