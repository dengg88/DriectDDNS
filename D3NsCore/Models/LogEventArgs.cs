using System;

namespace D3NsCore.Models
{
    public class LogEventArgs : EventArgs
    {
        public LogLevel Level { get; set; }
        public string Log { get; set; }
    }
}