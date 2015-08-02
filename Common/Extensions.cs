using Serilog;
using System;
using System.IO;
using System.Linq;

namespace Common
{
    public static class Extensions
    {
        private static ILogger _traceLog;
        public static ILogger TraceLog
        {
            get
            {
                if (_traceLog == null)
                {
                    _traceLog = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            typeof(Extensions).ToString().Split('.').First() + ".log"))
                        .CreateLogger();
                }

                return _traceLog;
            }
        }

        private static ILogger _errorLog;
        public static ILogger ErrorLog
        {
            get
            {
                if (_errorLog == null)
                {
                    _errorLog = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            typeof(Extensions).ToString().Split('.').First() + "-error.log"))
                        .CreateLogger();
                }

                return _errorLog;
            }
        }

        public static string Quote(this string value, string append = "")
        {
            return string.Format("\"{0}\"{1}", value, append);
        }
    }
}
