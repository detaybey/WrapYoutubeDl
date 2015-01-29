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

        public delegate void FinishedDownloadEventHandler(object sender);
        public event FinishedDownloadEventHandler FinishedDownload;

        public delegate void StartedDownloadEventHandler(object sender);
        public event StartedDownloadEventHandler StartedDownload;

        public delegate void ErrorEventHandler(object sender, ProgressEventArgs e);
        public event ErrorEventHandler ErrorDownload;

        public bool Started { get; set; }
        public bool Finished { get; set; }
        public decimal Percentage { get; set; }
        public Process Process { get; set; }
        public string OutputName { get; set; }
        public string DestinationFolder { get; set; }
        public AudioDownloader(string url, string outputName, string outputfolder)
        {
            OutputName = outputName + ".mp3";
            DestinationFolder = outputfolder;

            // this is the path where you keep the binaries (ffmpeg, youtube-dl etc)
            var binaryPath = ConfigurationManager.AppSettings["binaryfolder"];
            var arguments = string.Format(@"--extract-audio --audio-format mp3 {0} -o ""{1}""", url, OutputName);

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
        protected virtual void OnDownloadFinished()
        {
            if (FinishedDownload != null)
            {
                FinishedDownload(this);
            }
        }

        protected virtual void OnDownloadStarted()
        {
            if (StartedDownload != null)
            {
                StartedDownload(this);
            }
        }

        protected virtual void OnDownloadError(ProgressEventArgs e)
        {
            if (ErrorDownload != null)
            {
                ErrorDownload(this, e);
            }
        }

        public void Download()
        {
            Process.Start();
            Process.BeginOutputReadLine();
            this.OnDownloadStarted();
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
            var pattern = new Regex(@"\b\d+([\.,]\d+)?", RegexOptions.None);
            if (String.IsNullOrEmpty(outLine.Data))
            {
                return;
            }
            if (!outLine.Data.Contains("[download]"))
            {
                return;
            }
            if (!pattern.IsMatch(outLine.Data))
            {
                return;
            }

            // fire the process event
            var perc = Convert.ToDecimal(Regex.Match(outLine.Data, @"\b\d+([\.,]\d+)?"));
            this.OnProgress(new ProgressEventArgs() { Percentage = perc });

            // is it finished?
            if (perc < 100 || Finished)
            {
                return;
            }

            // task is finished, move the file to destination
            System.IO.File.Move(OutputName, System.IO.Path.Combine(DestinationFolder, OutputName));
            this.OnDownloadFinished();
            Finished = true;
        }
    }

}
