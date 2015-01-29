# WrapYoutubeDl

C# wrapper for https://github.com/rg3/youtube-dl 

Download audio from web using c#

#Version

1.0.2

# Installation

You need to download or use the binaries in the repo, following exe files are needed;

* ffmpeg.exe  - https://www.ffmpeg.org/download.html
* youtube-dl.exe   - https://rg3.github.io/youtube-dl/download.html

# Setup

Within app.config or web.config file, under your <appSettings> please add a key/value pair for your binaries path.
```xml
  <appSettings>
    <add key="binaryfolder" value="PATH_TO_YOUR_BINARIES"/>
  </appSettings>
```  

# Usage
```c#
  static void Main(string[] args)
  {
    var urlToDownload = "https://www.youtube.com/watch?v=JLCybxJU4qM";
    var newFilename = Guid.NewGuid().ToString();
    var mp3OutputFolder = "c:/@mp3/";

    var downloader = new AudioDownloader(urlToDownload, newFilename, mp3OutputFolder);
    downloader.ProgressDownload += downloader_ProgressDownload;
    downloader.FinishedDownload += downloader_FinishedDownload;
    downloader.Download();

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
```
# NuGet

To install WrapYouTubeDl, run the following command in the Package Manager Console
```sh
PM> Install-Package WrapYouTubeDl
```

# License

Apache License 2.0
