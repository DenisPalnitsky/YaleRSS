using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            LecturesScraper scraper = new LecturesScraper();
            scraper.ScrapeLectures();

            Console.WriteLine("Done. Press any key to close the window");
            Console.ReadKey();
        }


        public static void GetLinksFromUrl()
        {
            var l = AllStringBetweenTwo("a cc ba cc b", "a", "b").ToList();
            Console.WriteLine(string.Join(" ",  l));
        }

        //private static void GetMediaLinksFromLecturesLinks(List<string> urls)
        //{
        //    var links = GetAllLinksToMedia(urls, "spring07/psyc110");

        //    foreach (var l in links)
        //    {
        //        var path = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(l.Substring(l.IndexOf(@"/mp3/"))));
        //        Directory.CreateDirectory(Path.GetDirectoryName(path));

        //        using (FileStream f = new FileStream(path, FileMode.Create))
        //        {
        //            YaleSiteRequest.GetFile(l).GetResponseStream().CopyTo(f);
        //        }
        //    }
        //}

        public static LectureEntity[] GetAllLinksToMedia(System.Collections.Generic.List<string> urls, string lectureDirectory )
        {
            HttpClient http = new HttpClient();

            //if (File.Exists(OUTPUT_FILE_NAME))
            //    File.Delete(OUTPUT_FILE_NAME);

            List<LectureEntity> lectures =  new List<LectureEntity>();
            int order  = 1;
            foreach (var url in urls)
            {
                try
                {
                    var t = http.GetStringAsync(url);
                    t.Wait();
                 
                    var r = t.Result;

                    var result = new LectureEntity
                    {
                        Overview = StringBetweenTwo(r, @"Overview</h3>", "</p>"),
                        LectureId = StringBetweenTwo(r, $"media_downloader.cgi?file=/courses/{lectureDirectory}/mp3/", ".mp3"),
                        LectureNumber = StringBetweenTwo(r, @"session-number"">", "</h2>"),
                        Name = StringBetweenTwo(r, @"session-title"">&nbsp;-&nbsp;", "</h2>"),
                        Order = order
                    };

                    File.AppendAllText(OUTPUT_FILE_NAME, JsonConvert.SerializeObject(result) + ", \n");

                    order++;
                    //lectures.Add($"http://openmedia.yale.edu/cgi-bin/open_yale/media_downloader.cgi?file=/courses/{ lectureDirectory }/mp3/{ result.LectureId }.mp3");
                    lectures.Add(result);
                }
                catch (ArgumentOutOfRangeException) { }
                //catch (HttpRequestException)
                catch (AggregateException ex) when (ex.InnerExceptions[0] is System.Net.Http.HttpRequestException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    return new LectureEntity[0];
                }
            }

            return lectures.ToArray();
        }

        public static string StringBetweenTwo(string str, string before, string after)
        {
            return AllStringBetweenTwo(str, before, after).Single();
        }

        public static IEnumerable<string> AllStringBetweenTwo(string str, string before, string after)
        {
            int beginning = 0;
            int start;
            do
            {
                start = str.IndexOf(before, beginning);

                if (start == -1)
                    break;

                var index2 = str.IndexOf(after, start + before.Length);
                var length = index2 - start - before.Length;
                beginning = start + length ;

                yield return str.Substring(start+before.Length, length );
            }
            while (true);
        }
    }
}
