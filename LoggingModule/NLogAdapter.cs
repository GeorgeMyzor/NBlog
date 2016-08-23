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
        private readonly NLog.Logger nLogger = LogManager.GetCurrentClassLogger();

        public void Trace(Exception exception)
        {
            nLogger.Trace(exception);
        }

        public void Trace(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Trace(output);
        }

        public void Debug(Exception exception)
        {
            nLogger.Debug(exception);
        }

        public void Debug(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Debug(output);
        }

        public void Info(Exception exception)
        {
            nLogger.Info(exception);
        }

        public void Info(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Info(output);
        }

        public void Warn(Exception exception)
        {
            nLogger.Warn(exception);
        }

        public void Warn(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Warn(output);
        }

        public void Error(Exception exception)
        {
            nLogger.Error(exception);
        }

        public void Error(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Error(output);
        }

        public void Fatal(Exception exception)
        {
            nLogger.Fatal(exception);
        }

        public void Fatal(string format, params object[] args)
        {
            var output = string.Format(format, args);
            nLogger.Fatal(output);
        }
    }
}
