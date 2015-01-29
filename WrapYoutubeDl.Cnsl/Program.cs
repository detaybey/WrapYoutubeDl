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

            var downloader = new AudioDownloader("https://www.youtube.com/watch?v=qDc_5zpBj7s", Guid.NewGuid().ToString(), "c:/@mp3/");
            downloader.ProgressDownload += downloader_ProgressDownload;
            downloader.Download();
            Console.ReadLine();
        }

        static void downloader_ProgressDownload(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.Percentage);
        }
 
    }
}
