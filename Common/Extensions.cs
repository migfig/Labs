using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                        .MinimumLevel.Is(LogLevel)                       
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
                        .MinimumLevel.Is(LogEventLevel.Error)
                        .CreateLogger();
                }

                return _errorLog;
            }
        }

        public static LogEventLevel LogLevel
        {
            get { return (LogEventLevel)Enum.Parse(typeof(LogEventLevel), Properties.Settings.Default.DefaultLogLevel); }
            set
            {
                if (value.ToString() == Properties.Settings.Default.DefaultLogLevel) return;
                Properties.Settings.Default.DefaultLogLevel = value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        public static List<LogEventLevel> LogLevels
        {
            get
            {
                var list = new List<LogEventLevel>();
                foreach (var item in Properties.Settings.Default.LogLevels)
                    list.Add((LogEventLevel)Enum.Parse(typeof(LogEventLevel), item));

                return list;
            }
        }

        public static string Quote(this string value, string append = "")
        {
            return string.Format("\"{0}\"{1}", value, append);
        }

        public static int runProcess(string program, string args, int waitForExitMS = 30000)
        {
            TraceLog.Information("Running method with args {program} {args}", program, args);
            System.Diagnostics.Debug.WriteLine(program + " " + args);

            var p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = program,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();

            if (waitForExitMS > 0)
            {
                p.WaitForExit(waitForExitMS);
                return p.ExitCode;
            }

            return 0;
        }
    }

    public static class ObjectExtensions
    {
        public static bool _IsEqualTo(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) == Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source) == Convert.ToBase64String((Byte[])_target);
                case "System.Int32":
                    return Convert.ToInt32(_source) == Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) == Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) == Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) == Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) == Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) == Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) == Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source) == Convert.ToString(_target);
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) == Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source) == Convert.ToString(_target);
            }

            return _source == _target;
        }

        public static bool _IsNotEqualTo(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) != Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source) != Convert.ToBase64String((Byte[])_target);
                case "System.Int32":
                    return Convert.ToInt32(_source) != Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) != Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) != Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) != Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) != Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) != Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) != Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source) != Convert.ToString(_target);
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) != Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source) != Convert.ToString(_target);
            }

            return _source == _target;
        }

        public static bool _IsGreaterThan(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) != Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source).Length > Convert.ToBase64String((Byte[])_target).Length;
                case "System.Int32":
                    return Convert.ToInt32(_source) > Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) > Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) > Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) > Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) > Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) > Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) > Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source).Length > Convert.ToString(_target).Length;
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) > Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source).Length > Convert.ToString(_target).Length;
            }

            return _source != _target;
        }

        public static bool _IsLessThan(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) != Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source).Length < Convert.ToBase64String((Byte[])_target).Length;
                case "System.Int32":
                    return Convert.ToInt32(_source) < Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) < Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) < Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) < Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) < Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) < Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) < Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source).Length < Convert.ToString(_target).Length;
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) < Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source).Length < Convert.ToString(_target).Length;
            }

            return _source != _target;
        }

        public static bool _IsGreaterThanOrEqual(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) != Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source).Length >= Convert.ToBase64String((Byte[])_target).Length;
                case "System.Int32":
                    return Convert.ToInt32(_source) >= Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) >= Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) >= Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) >= Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) >= Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) >= Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) >= Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source).Length >= Convert.ToString(_target).Length;
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) >= Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source).Length >= Convert.ToString(_target).Length;
            }

            return _source != _target;
        }

        public static bool _IsLessThanOrEqual(this object _source, object _target)
        {
            switch (_source.GetType().ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(_source) != Convert.ToBoolean(_target);
                case "System.Byte[]":
                    return Convert.ToBase64String((Byte[])_source).Length <= Convert.ToBase64String((Byte[])_target).Length;
                case "System.Int32":
                    return Convert.ToInt32(_source) <= Convert.ToInt32(_target);
                case "System.Int64":
                    return Convert.ToInt64(_source) <= Convert.ToInt64(_target);
                case "System.Decimal":
                    return Convert.ToDecimal(_source) <= Convert.ToDecimal(_target);
                case "System.Double":
                    return Convert.ToDouble(_source) <= Convert.ToDouble(_target);
                case "System.Single":
                    return Convert.ToSingle(_source) <= Convert.ToSingle(_target);
                case "System.Int16":
                    return Convert.ToInt16(_source) <= Convert.ToInt16(_target);
                case "System.Byte":
                    return Convert.ToByte(_source) <= Convert.ToByte(_target);
                case "System.String":
                    return Convert.ToString(_source).Length <= Convert.ToString(_target).Length;
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                    return Convert.ToDateTime(_source) <= Convert.ToDateTime(_target);
                case "System.Guid":
                    return Convert.ToString(_source).Length <= Convert.ToString(_target).Length;
            }

            return _source != _target;
        }
    }
}
