using System;
using Topshelf;

namespace Log.Service
{
    public class LogService: ServiceControl
    {
        private ILogServices _logServices;
        public LogService(ILogServices logServices)
        {
            _logServices = logServices;
        }

        public bool Start(HostControl host)
        {
            var entries = _logServices.GetEntries().GetAwaiter().GetResult();
            foreach (var entry in entries)
            {
                Console.WriteLine("{0}\t{1}\t{2}", entry.TimeStamp.ToString("MM-DD hh:mm:ss"), entry.Message, entry.EventLevel.ToString());
            }

            Console.WriteLine("Press Enter key to continue...");
            Console.ReadLine();
            return true;
        }

        public bool Stop(HostControl host)
        {
            return true;         
        }
    }
}
