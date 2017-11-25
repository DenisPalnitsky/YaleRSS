using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace YaleRSS.Controllers
{


    public class TestYaleRequestController : ApiController
    {
        [Route("TestLecture")]
        [HttpGet]
        public HttpResponseMessage GetLecture()
        {            
            HttpWebRequest request = WebRequest.CreateHttp("http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3");
            request.Referer = @"http://oyc.yale.edu/courses/";
           
            var receivedResponse = request.GetResponse();

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);                    
            response.Content = new StreamContent(receivedResponse.GetResponseStream());
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(receivedResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "phil181_01_011111.mp3";
            response.Content.Headers.ContentLength = receivedResponse.ContentLength;

            return response;
        }


        [Route("FromTempFile")]
        [HttpGet]
        public HttpResponseMessage GetFromTempFile()
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3");
            request.Referer = @"http://oyc.yale.edu/courses/";

            var receivedResponse = request.GetResponse();
            

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string filename = "phil181_01_011111.mp3";
            string tempFilePath = GetTempFilePath(filename);

            Trace.WriteLine("reponse received");
            if (!System.IO.File.Exists(tempFilePath))
            {
                Trace.WriteLine("Saving to temp file");
                SaveTempFile(receivedResponse.GetResponseStream(), tempFilePath);
                
            }

            Trace.WriteLine("Returning from temp file");
            response.Content = new StreamContent(GetTempFile(tempFilePath));
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(receivedResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filename;
            response.Content.Headers.ContentLength = receivedResponse.ContentLength;

            return response;
        }

        [Route("GetFromMemoryFile")]
        [HttpGet]
        public HttpResponseMessage GetFromMemoryFile()
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3");
            request.Referer = @"http://oyc.yale.edu/courses/";

            var receivedResponse = request.GetResponse();

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string filename = "phil181_01_011111.mp3";


            Trace.WriteLine("Response received");
            MemoryStream ms = new MemoryStream();
            receivedResponse.GetResponseStream().CopyTo(ms);
            Trace.WriteLine("Stream copied");

            ms.Seek(0, SeekOrigin.Begin);

            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(receivedResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filename;
            response.Content.Headers.ContentLength = receivedResponse.ContentLength;

            Trace.WriteLine("Returning responce");
            return response;
        }

        
        [Route("FromDrive")]
        [HttpGet]
        public HttpResponseMessage GetLectureFromDrive()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://drive.google.com/uc?authuser=0&id=0Bw6jBqc1qDyrR0Y4ZUJaOFVLSlU&export=download");
            //request.Referer = @"http://oyc.yale.edu/courses/";

            var receivedResponse = request.GetResponse();

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(receivedResponse.GetResponseStream());
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(receivedResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "phil181_01_011111.mp3";
            response.Content.Headers.ContentLength = receivedResponse.ContentLength;
            try { 
            return response;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        [Route("JustImage")]
        [HttpGet]
        public HttpResponseMessage JustImage()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://drive.google.com/file/d/1-VxEMD_rL7roMjoMROkbFSUHRDBl8FZg/view?usp=sharing");

            var receivedResponse = request.GetResponse();

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(receivedResponse.GetResponseStream());
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(receivedResponse.ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "image.jpeg";
            response.Content.Headers.ContentLength = receivedResponse.ContentLength;
            try
            {
                return response;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        

        private void SaveTempFile(Stream fileStream, string filename )
        {
            string filePath = filename;

            //if the path is exists,delete old file
            if (System.IO.File.Exists(filePath))
            {
                return;
            }

            using (var filestream = File.OpenWrite(filePath))
            {
                fileStream.CopyTo(filestream);                
            }                               
        }

        private Stream GetTempFile(string filename)
        {
            string filePath = filename;
            
                return File.OpenRead(filePath);
                    }

        private static string GetTempFilePath(string filename)
        {
            string tempPath = Path.GetTempPath();
            string filePath = tempPath + filename;
            return filePath;
        }
    }
}
