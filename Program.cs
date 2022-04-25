using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using HttpClientProgress;

namespace Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Download... ");
            DownloadWebpage("https://tretton37.com/");
            ConsoleUtility.WriteProgressBar(0);
            Console.WriteLine("Finished Downloading");
        }
        public static int i = 1;

        public static async void DownloadWebpage(string url)
        {
            var progress = new Progress<float>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(2);

                    Console.WriteLine($"Downloading Website... {url}");
                    // var text = await client.GetStringAsync(new Uri(url));
                    progress.ProgressChanged += Progress_ProgressChanged;

                    // Console.WriteLine(text);
                    using (var file = new FileStream ($"C:\\Webpages\\{url.Split('/')[2]}" + i++ + ".txt", FileMode.Create, FileAccess.Write, FileShare.None))
		                await client.DownloadDataAsync(url, file, progress);

                    // await File.WriteAllTextAsync($"C:\\Webpages\\{url.Split('/')[2]}" + i++ + ".txt", text);
                    HtmlWeb hw = new HtmlWeb();
                    HtmlDocument doc = hw.Load(url);
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        string href = link.Attributes["href"].Value.ToString();
                        if (href.StartsWith("https"))
                        {
                            DownloadWebpage(href);
                        }
                    }
                    Console.WriteLine($"Website Downloaded... {url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download Website... {url} Error: {ex.Message}");
            }

        }
        public static void Progress_ProgressChanged(object sender, float progress)
        {
            ConsoleUtility.WriteProgressBar(progress, false);
        }     
    }
}
