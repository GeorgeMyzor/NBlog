using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LoggingModule
{
    public class NLogAdapter : ILogger
    {
        private static NLog.Logger NLogger = LogManager.GetCurrentClassLogger();

        public void Trace(Exception exception)
        {
            NLogger.Trace(exception);
        }

        public void Trace(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Trace(output);
        }

        public void Debug(Exception exception)
        {
            NLogger.Debug(exception);
        }

        public void Debug(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Debug(output);
        }

        public void Info(Exception exception)
        {
            NLogger.Info(exception);
        }

        public void Info(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Info(output);
        }

        public void Warn(Exception exception)
        {
            NLogger.Warn(exception);
        }

        public void Warn(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Warn(output);
        }

        public void Error(Exception exception)
        {
            NLogger.Error(exception);
        }

        public void Error(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Error(output);
        }

        public void Fatal(Exception exception)
        {
            NLogger.Fatal(exception);
        }

        public void Fatal(string format, params object[] args)
        {
            var output = string.Format(format, args);
            NLogger.Fatal(output);
        }
    }
}
