using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using YaleRss.Data;
using YaleRss.Data.WebData;

namespace YaleRss.LecturesDataExtractor
{
    class Program
    {
        private const string OUTPUT_FILE_NAME = "lectures.json";

        static void Main(string[] args)
        {
            System.Collections.Generic.List<string> urls = new System.Collections.Generic.List<string>() {
            "http://oyc.yale.edu/philosophy/phil-181/lecture-1",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-2",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-3",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-4",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-5",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-6",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-7",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-8",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-9",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-10",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-11",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-12",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-13",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-14",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-15",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-16",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-17",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-18",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-19",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-20",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-21",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-22",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-23",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-24",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-25",
            "http://oyc.yale.edu/philosophy/phil-181/lecture-26"
            };

            var links =  GetAllLinksToMedia(urls);

            foreach (var l in links)
            {
                var path = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(l.Substring(l.IndexOf(@"/mp3/"))));
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (FileStream f = new FileStream(path, FileMode.Create))
                {
                    YaleSiteRequest.GetFile(l).GetResponseStream().CopyTo(f);
                }
            }
        }

        private static string[] GetAllLinksToMedia(System.Collections.Generic.List<string> urls)
        {
            HttpClient http = new HttpClient();

            if (File.Exists(OUTPUT_FILE_NAME))
                File.Delete(OUTPUT_FILE_NAME);

            List<string> links =  new List<string>(); 

            foreach (var url in urls)
            {

                var t = http.GetStringAsync(url);
                t.Wait();

                var r = t.Result;

                var result = new
                {
                    Overview = StringBetweenTwo(r, @"Overview</h3>", "</p>"),
                    LectureId = StringBetweenTwo(r, @"media_downloader.cgi?file=/courses/spring11/phil181/mp3/", ".mp3"),
                    LectureNumber = StringBetweenTwo(r, @"session-number"">", "</h2>"),
                    Name = StringBetweenTwo(r, @"session-title"">&nbsp;-&nbsp;", "</h2>")
                };

                File.AppendAllText(OUTPUT_FILE_NAME, JsonConvert.SerializeObject(result) + ", \n");
                links.Add($"http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/spring11/phil181/mp3/{ result.LectureId }.mp3");
                Console.WriteLine(JsonConvert.SerializeObject(result));
            }

            return links.ToArray();
        }

        public static string StringBetweenTwo ( string str, string before, string after)
        {
            var start = str.IndexOf(before)+ before.Length;
            var length = str.Substring(start).IndexOf(after);

            return str.Substring(start, length);
        }

    }
}
