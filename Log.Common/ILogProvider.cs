using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Common
{
    public interface ILogProvider
    {
        string Name { get; }
        string Path { get; }
        Task<IEnumerable<LogEntry>> GetEntries();
        Task<IEnumerable<LogEntry>> GetEntries(TimeSpan span);
    }
}
