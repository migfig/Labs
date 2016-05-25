using Castle.Windsor;
using Topshelf;

namespace Log.Service
{
    public class LogService: ServiceControl
    {
        public static IWindsorContainer Container { get; set; }
        private ILogServices _logServices;
        private IServiceable _webApp;
        public LogService(ILogServices logServices, IServiceable webApp)
        {
            _logServices = logServices;
            _webApp = webApp;
        }

        public bool Start(HostControl host)
        {
            return _webApp.Start();
        }

        public bool Stop(HostControl host)
        {
            return _webApp.Stop();         
        }
    }
}
