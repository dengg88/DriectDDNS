using System;
using D3NsCore.Models;

namespace D3NsCore
{
    public abstract class BasicServ
    {
        public abstract void Start();

        public abstract void Stop();

        public event EventHandler<LogEventArgs> Log;

        protected virtual void OnLog(LogEventArgs e)
        {
            Log?.Invoke(this, e);
        }

        protected void LogDebug(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Debug, Log = message });
        }

        protected void LogTrace(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Trace, Log = message });
        }

        protected void LogInfo(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Info, Log = message });
        }

        protected void LogWarning(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Warning, Log = message });
        }

        protected void LogError(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Error, Log = message });
        }

        protected void LogFatal(string message)
        {
            OnLog(new LogEventArgs { Level = LogLevel.Fatal, Log = message });
        }
    }
}