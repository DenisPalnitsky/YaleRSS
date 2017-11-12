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
          
            var yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AudioUrlPattern, lecture.LectureId));
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            if (yaleResponse.StatusCode == HttpStatusCode.OK)
            {
                Trace.WriteLine("Attempt to get stream from Yale succeeded");
                response.Content = new StreamContent(yaleResponse.GetResponseStream());             
            }
            else
            {
                Trace.WriteLine("Attempt to get stream from Yale failed. Sourcing from storage");
                yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AlternativeAudioUrlPattern, lecture.LectureId));
            }

            Trace.WriteLine("Setting headers");
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(yaleResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = lecture.LectureId + ".mp3";
            response.Content.Headers.ContentLength = yaleResponse.ContentLength;

            Trace.WriteLine("Returning response");
            return response;
        }

        
        
    }
}
