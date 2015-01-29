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
            var urlToDownload = "https://www.youtube.com/watch?v=qDc_5zpBj7s";
            var newFilename = Guid.NewGuid().ToString();
            var mp3OutputFolder = "c:/@mp3/";

            var downloader = new AudioDownloader(urlToDownload, newFilename, mp3OutputFolder);
            downloader.ProgressDownload += downloader_ProgressDownload;
            downloader.FinishedDownload += downloader_FinishedDownload;
            downloader.Download();

            Console.ReadLine();
        }

        static void downloader_FinishedDownload(object sender)
        {
            Console.WriteLine("Finished!");
        }

        static void downloader_ProgressDownload(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.Percentage);
        }

    }
}
