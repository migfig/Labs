using Castle.Windsor;
using System;
using Topshelf;

namespace Log.Service
{
    public class LogService: ServiceControl
    {
        public static IWindsorContainer Container { get; set; }
        private ILogServices _logServices;
        private IStartable _webApp;
        public LogService(ILogServices logServices, IStartable webApp)
        {
            _logServices = logServices;
            _webApp = webApp;
        }

        public bool Start(HostControl host)
        {
            _webApp.Start();

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
