using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrapYoutubeDl;

namespace WrapYoutubeDl.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {           
          
            var mp3OutputFolder = "c:/@mp3/";

            var downloader = new AudioDownloader("https://www.youtube.com/watch?v=efgIm9YPZvE", Guid.NewGuid().ToString(), mp3OutputFolder, false);
            downloader.ProgressDownload += downloader_ProgressDownload;
            downloader.FinishedDownload += downloader_FinishedDownload;
            downloader.Download();

            var downloader2 = new AudioDownloader("https://www.youtube.com/watch?v=09R8_2nJtjg", Guid.NewGuid().ToString(), mp3OutputFolder, false);
            downloader2.ProgressDownload += downloader_ProgressDownload;
            downloader2.FinishedDownload += downloader_FinishedDownload;
            downloader2.Download();

            Console.ReadLine();
        }

        static void downloader_FinishedDownload(object sender, DownloadEventArgs e)
        {
            Console.WriteLine("Finished!");
        }

        static void downloader_ProgressDownload(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.Percentage);
        }

    }
}
