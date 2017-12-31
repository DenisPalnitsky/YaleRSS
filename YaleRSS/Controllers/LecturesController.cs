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
        ICourseRepository _coursesRepository;
        IConfigurationRepository _configurationRepository;

        public LecturesController (ICourseRepository courseRepository, IConfigurationRepository configurationRepository)
        {
            _coursesRepository = courseRepository;
            _configurationRepository = configurationRepository;
        }

        [HttpGet("rss/lectures/{courseId}/{id}.mp3", Name=RouteNames.Lectures )]
        public IActionResult GetLectures(string courseId, string id)
        {            
            Trace.WriteLine($"Downloading lecture { id }");
            var course = _coursesRepository.GetCourse(courseId);

            var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));

            if (!String.IsNullOrEmpty(course.InternalUrlPattern))
            {
                Trace.WriteLine("Redirecting to Azure storage");
                return RedirectPreserveMethod(String.Format(course.InternalUrlPattern, lecture.LectureId));
            }
        
            Trace.WriteLine("There is no media file in Azure storage. Pulling file from Yale");
            var yaleResponse = YaleSiteRequest.GetFile(String.Format(_configurationRepository.GetConfiguration().YaleUrlBase + course.YaleUrlPattern, lecture.LectureId));
       
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
