using System;
using NLog;

namespace Messy.Logging
{
    internal class NLogLogger : ILogger
    {
        private readonly Logger _nlogLogger;

        public NLogLogger(Type type)
        {
            _nlogLogger = LogManager.GetLogger(type.FullName);
        }

        public void Debug(string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Debug, format, args);
        }

        public void Info(string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Info, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Warn, format, args);
        }

        public void Error(string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Error, format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Error, ex, format, args);
        }

        public void Fatal(Exception ex, string format, params object[] args)
        {
            _nlogLogger.Log(LogLevel.Fatal, ex, format, args);
        }
    }
}
