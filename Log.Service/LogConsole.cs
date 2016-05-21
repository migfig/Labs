using Log.Common;
using Log.Provider.Default;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Service
{
    public class LogConsole
    {
        private static ILogServices _logServices;

        static void Main()
        {
            _logServices = new LogServices(new CustomFileProvider(ConfigurationManager.AppSettings["LogPath"], ConfigurationManager.AppSettings["LogName"]));

            var entries = _logServices.GetEntries().GetAwaiter().GetResult();
            foreach(var entry in entries)
            {
                Console.WriteLine("{0}\t{1}\t{2}", entry.TimeStamp.ToString("MM-DD hh:mm:ss"), entry.Message, entry.EventLevel.ToString());
            }

            Console.WriteLine("Press Enter key to continue...");
            Console.ReadLine();
        }
    }
}
