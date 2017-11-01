using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YaleRSS.Data.WebData
{
    public class YaleSiteRequest
    {

        public static Stream GetFile (string url)
        {
            // Original Request
            // GET http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3 HTTP/1.1
            // Host: openmedia.yale.edu
            // Connection: keep - alive
            // Pragma: no - cache
            // Cache - Control: no - cache
            // Upgrade - Insecure - Requests: 1
            // User - Agent: Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 61.0.3163.100 Safari / 537.36
            // Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8
            // Referer: http://oyc.yale.edu/philosophy/phil-181/lecture-1
            // Accept-Encoding: gzip, deflate
            // Accept-Language: pl-PL,pl;q=0.8,en-US;q=0.6,en;q=0.4,ru;q=0.2,ro;q=0.2
            // Cookie: _ga=GA1.2.2081577969.1500935226


            //string url = @"http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/phil181_01_011111.mp3";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Referer = "http://oyc.yale.edu/courses/";

            var response = request.GetResponse();
            return response.GetResponseStream();
                      
        }

    }
}
