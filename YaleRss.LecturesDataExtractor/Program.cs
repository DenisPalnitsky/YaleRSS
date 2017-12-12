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
        private const string OUTPUT_FILE_NAME = "psyc110.json";

        static void Main(string[] args)
        {
            System.Collections.Generic.List<string> urls = Urls.psyc_110;

            GetMediaLinksFromLecturesLinks(urls);

            Console.WriteLine("Done. Press any key to close the window");
            Console.ReadKey();
        }

        private static void GetMediaLinksFromLecturesLinks(List<string> urls)
        {
            var links = GetAllLinksToMedia(urls, "spring07/psyc110");

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

        private static string[] GetAllLinksToMedia(System.Collections.Generic.List<string> urls, string lectureDirectory )
        {
            HttpClient http = new HttpClient();

            if (File.Exists(OUTPUT_FILE_NAME))
                File.Delete(OUTPUT_FILE_NAME);

            List<string> links =  new List<string>(); 

            foreach (var url in urls)
            {
                try
                {
                    var t = http.GetStringAsync(url);
                    t.Wait();

                    var r = t.Result;

                    var result = new
                    {
                        Overview = StringBetweenTwo(r, @"Overview</h3>", "</p>"),
                        LectureId = StringBetweenTwo(r, $"media_downloader.cgi?file=/courses/{lectureDirectory}/mp3/", ".mp3"),
                        LectureNumber = StringBetweenTwo(r, @"session-number"">", "</h2>"),
                        Name = StringBetweenTwo(r, @"session-title"">&nbsp;-&nbsp;", "</h2>")
                    };

                    File.AppendAllText(OUTPUT_FILE_NAME, JsonConvert.SerializeObject(result) + ", \n");


                    links.Add($"http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/{ lectureDirectory }/mp3/{ result.LectureId }.mp3");
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }
                catch (ArgumentOutOfRangeException) { }
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
