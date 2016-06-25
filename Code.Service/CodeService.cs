using Castle.Windsor;
using Topshelf;

namespace Code.Service
{
    public class CodeService: ServiceControl
    {
        public static IWindsorContainer Container { get; set; }
        private ICodeServices _codeServices;
        private IServiceable _webApp;
        public CodeService(ICodeServices codeServices, IServiceable webApp)
        {
            _codeServices = codeServices;
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
