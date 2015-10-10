using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrapYoutubeDl;

namespace WrapYoutubeDl.Cnsl
{
    class song
    {
        public string name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {           
          
            var mp3OutputFolder = "c:/@mp3/";

            var downloader = new AudioDownloader("https://www.youtube.com/watch?v=pcS9a_Eup5Y", Guid.NewGuid().ToString(), mp3OutputFolder, false);
            downloader.ProcessObject = new song() { name = "hede" };
            downloader.ProgressDownload += downloader_ProgressDownload;
            downloader.FinishedDownload += downloader_FinishedDownload;
            downloader.Download();
        

            Console.ReadLine();
        }

        static void downloader_FinishedDownload(object sender, DownloadEventArgs e)
        {
            var song = e.ProcessObject as song;
            Console.WriteLine("Finished {0}", song.name);
        }

        static void downloader_ProgressDownload(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("[progress {0}]", e.Percentage);
        }

    }
}
