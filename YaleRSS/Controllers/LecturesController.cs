using System;
using System.Collections.Generic;
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

        //[Route("Lectures/{courseId}/{lectureId}")]
        public HttpResponseMessage GetLectures(string id)
        {
            var course = _repo.Philosophy;
            var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));
           
            var yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AudioUrlPattern, lecture.LectureId));

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(yaleResponse.GetResponseStream());
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(yaleResponse.ContentType);            
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = lecture.LectureId + ".mp3";
            response.Content.Headers.ContentLength = yaleResponse.ContentLength;            
            return response;
        }


        
    }
}
