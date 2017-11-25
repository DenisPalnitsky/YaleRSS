using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YaleRSS.Data;

namespace YaleRSS.Controllers
{
    public class LecturesController : ApiController
    {
        CourseRepository _repo = new CourseRepository(DbContext.Create());


        //public HttpResponseMessage GetLectures(string id)
        //{
        //    var course = _repo.Philosophy;
        //    var lecture = course.Lectures.Single(l => l.LectureId.Equals(id, StringComparison.CurrentCultureIgnoreCase));

        //    Trace.WriteLine("Getting file from Yale server " + String.Format(course.AudioUrlPattern, lecture.LectureId));
        //    var yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AudioUrlPattern, lecture.LectureId));


        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

        //    //if (yaleResponse.StatusCode == HttpStatusCode.OK)
        //    //{
        //    //    Trace.WriteLine("Response from Yale server received");
        //    //             
        //    //}
        //    //else
        //    {
        //        Trace.WriteLine($"Yale server responded { yaleResponse.StatusCode }");
        //        Trace.WriteLine($"Using Alternative url  { String.Format(course.AlternativeAudioUrlPattern, lecture.LectureId) }");
        //        yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AlternativeAudioUrlPattern, lecture.LectureId));

        //    }

        //    response.Content = new StreamContent(yaleResponse.GetResponseStream());
        //    response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(yaleResponse.ContentType);
        //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = lecture.LectureId + ".mp3";
        //    response.Content.Headers.ContentLength = yaleResponse.ContentLength;

        //    Trace.WriteLine($"Returning response");
        //    return response;
        //}


        public class FileActionResult : IHttpActionResult
        {
            private readonly CourseRepository _repo;

            public FileActionResult(CourseRepository repo,  string fileId)
            {
                this.FileId = fileId;
                _repo = repo;
            }

            public string FileId { get; private set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var course = _repo.Philosophy;
                var lecture = course.Lectures.Single(l => l.LectureId.Equals(FileId, StringComparison.CurrentCultureIgnoreCase));

                Trace.WriteLine("Getting file from Yale server " + String.Format(course.AudioUrlPattern, lecture.LectureId));
                var yaleResponse = YaleRSS.Data.WebData.YaleSiteRequest.GetFile(String.Format(course.AudioUrlPattern, lecture.LectureId));


                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                if (yaleResponse.StatusCode == HttpStatusCode.OK)
                {
                    Trace.WriteLine("Response from Yale server received");
                }

                Trace.WriteLine("Set stream as response");
                response.Content = new StreamContent(yaleResponse.GetResponseStream());
                response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(yaleResponse.ContentType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = $"{lecture.LectureId}.mp3";
                response.Content.Headers.ContentLength = yaleResponse.ContentLength;

                Trace.WriteLine($"Returning response");
        

                return Task.FromResult(response);
            }
        }

        public IHttpActionResult GetLectures(string id)
        {
            return  new FileActionResult(_repo, id);
        }

    }
}
