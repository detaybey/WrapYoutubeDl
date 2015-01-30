using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrapYoutubeDl
{
    /// <summary>
    /// The event arguement class for passing when progress event is fired
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        public object ProcessObject { get; set; }
        public decimal Percentage { get; set; }
        public string Error { get; set; }

        public ProgressEventArgs() :
            base()
        {

        }

    }

    /// <summary>
    /// The event arguement class for passing when progress event is fired
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        public object ProcessObject { get; set; }
        public DownloadEventArgs() :
            base()
        {

        }

    }
}
