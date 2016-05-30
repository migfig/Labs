using Log.Common;
using Newtonsoft.Json;

namespace Log.Provider.Default
{
    public interface IEntryProvider
    {
        LogEntry GetEntry(string line);
    }

    public class DefaultEntryProvider: IEntryProvider
    {
        public LogEntry GetEntry(string line)
        {
            return JsonConvert.DeserializeObject<LogEntry>(line);
        }
    }
}
