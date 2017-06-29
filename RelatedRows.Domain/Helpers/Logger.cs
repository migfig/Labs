using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace RelatedRows.Domain
{
    public static class Logger
    {
        private static ILogger _log;
        public static ILogger Log
        {
            get
            {
                return _log ?? (
                    _log = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trace.log"))
                        .MinimumLevel.Is(LogEventLevel.Verbose)
                        .CreateLogger());
            }
        }

        public static void SetLevel(LogEventLevel level)
        {            
            _log = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trace.log"))
                        .MinimumLevel.Is(level)
                        .CreateLogger();
        }
    }
}
