using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WrapYoutubeDl
{
    public class AudioDownloader
    {
        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
        public event ProgressEventHandler ProgressDownload;

        public delegate void FinishedDownloadEventHandler(object sender, DownloadEventArgs e);
        public event FinishedDownloadEventHandler FinishedDownload;

        public delegate void StartedDownloadEventHandler(object sender, DownloadEventArgs e);
        public event StartedDownloadEventHandler StartedDownload;

        public delegate void ErrorEventHandler(object sender, ProgressEventArgs e);
        public event ErrorEventHandler ErrorDownload;

        public Object Object { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public decimal Percentage { get; set; }
        public Process Process { get; set; }
        public string OutputName { get; set; }
        public string DestinationFolder { get; set; }
        public AudioDownloader(string url, string outputName, string outputfolder)
        {

            DestinationFolder = outputfolder;

            // make sure filename ends with an mp3 extension
            OutputName = outputName;
            if (!OutputName.ToLower().EndsWith(".mp3"))
            {
                OutputName += ".mp3";
            }

            // this is the path where you keep the binaries (ffmpeg, youtube-dl etc)
            var binaryPath = ConfigurationManager.AppSettings["binaryfolder"];
            var arguments = string.Format(@"--continue --ignore-errors --no-overwrites --restrict-filenames --extract-audio --audio-format mp3 {0} -o ""{1}""", url, OutputName);

            this.Started = false;
            this.Finished = false;
            this.Percentage = 0;

            // setup the process that will fire youtube-dl
            Process = new Process();
            Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.StartInfo.FileName = System.IO.Path.Combine(binaryPath, "youtube-dl.exe");
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.Arguments = arguments;
            Process.StartInfo.CreateNoWindow = true;
            Process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            Process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceived);

        }

        protected virtual void OnProgress(ProgressEventArgs e)
        {
            if (ProgressDownload != null)
            {
                ProgressDownload(this, e);
            }
        }
        protected virtual void OnDownloadFinished(DownloadEventArgs e)
        {
            if (FinishedDownload != null)
            {
                FinishedDownload(this, e);
            }
        }

        protected virtual void OnDownloadStarted(DownloadEventArgs e)
        {
            if (StartedDownload != null)
            {
                StartedDownload(this, e);
            }
        }

        protected virtual void OnDownloadError(ProgressEventArgs e)
        {
            if (ErrorDownload != null)
            {
                ErrorDownload(this, e);
            }
        }

        public void Download(object obj  = null)
        {
            Process.Start();
            Process.BeginOutputReadLine();
            this.Object = obj;
            this.OnDownloadStarted(new DownloadEventArgs() { Object = obj });
        }

        public void ErrorDataReceived(object sendingprocess, DataReceivedEventArgs error)
        {
            if (!String.IsNullOrEmpty(error.Data))
            {
                this.OnDownloadError(new ProgressEventArgs() { Error = error.Data });
            }
        }
        public void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // extract the percentage from process output
            if (String.IsNullOrEmpty(outLine.Data))
            {
                return;
            }
            if (!outLine.Data.Contains("[download]"))
            {
                return;
            }
            var pattern = new Regex(@"\b\d+([\.,]\d+)?", RegexOptions.None);
            if (!pattern.IsMatch(outLine.Data))
            {
                return;
            }

            // fire the process event
            var perc = Convert.ToDecimal(Regex.Match(outLine.Data, @"\b\d+([\.,]\d+)?").Value);
            if (perc > 100 || perc < 0)
            {
                return;
            }
            this.Percentage = perc;
            this.OnProgress(new ProgressEventArgs() { Percentage = perc });

            // is it finished?
            if (perc < 100 || Finished)
            {
                return;
            }

            // task is finished, move the file to destination
            System.IO.File.Move(OutputName, System.IO.Path.Combine(DestinationFolder, OutputName));
            this.OnDownloadFinished(new DownloadEventArgs() { Object = this.Object });
            this.Finished = true;
        }
    }

}
