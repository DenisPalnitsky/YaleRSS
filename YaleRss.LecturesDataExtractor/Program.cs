using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;

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


            HttpClient http = new HttpClient();

            if (File.Exists(OUTPUT_FILE_NAME))
                File.Delete(OUTPUT_FILE_NAME);

            foreach (var url in urls) {

                var t = http.GetStringAsync(url);
                t.Wait();

                var r = t.Result;

                var result = new {
                    Overview = StringBetweenTwo(r, @"Overview</h3>", "</p>"),
                    LectureId = StringBetweenTwo(r, @"media_downloader.cgi?file=/courses/spring11/phil181/mp3/", ".mp3"),
                    LectureNumber = StringBetweenTwo(r,@"session-number"">", "</h2>"),
                    Name = StringBetweenTwo (r, @"session-title"">&nbsp;-&nbsp;", "</h2>")
                };

                File.AppendAllText(OUTPUT_FILE_NAME, JsonConvert.SerializeObject(result) + ", \n");

                Console.WriteLine(JsonConvert.SerializeObject(result));
            }
        }

        public static string StringBetweenTwo ( string str, string before, string after)
        {
            var start = str.IndexOf(before)+ before.Length;
            var length = str.Substring(start).IndexOf(after);

            return str.Substring(start, length);
        }
    }
}
