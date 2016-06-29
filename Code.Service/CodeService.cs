using Castle.Windsor;
using Common.Generics;
using Topshelf;
using Trainer.Domain;

namespace Code.Service
{
    public class CodeService: ServiceControl
    {
        public static IWindsorContainer Container { get; set; }
        private readonly ICodeServices _codeServices;
        private readonly IGenericServices<Presentation> _presentationServices;
        private readonly IServiceable _webApp;

        public CodeService(ICodeServices codeServices, IGenericServices<Presentation> presentationServices, IServiceable webApp)
        {
            _codeServices = codeServices;
            _presentationServices = presentationServices;
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
