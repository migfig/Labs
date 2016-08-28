using Castle.Windsor;
using Common.Generics;
using System;
using Topshelf;
using Trainer.Domain;
using Common.Controllers;

namespace Code.Service
{
    public class CodeService: ServiceControl
    {
        public static IWindsorContainer Container { get; set; }
        private readonly ICodeServices _codeServices;
        private readonly IGenericServices<Presentation> _presentationServices;
        private readonly IServiceable _webApp;
        private readonly IContentProvider<Presentation> _contentProvider;

        public CodeService(ICodeServices codeServices, IGenericServices<Presentation> presentationServices, IServiceable webApp, IContentProvider<Presentation> contentProvider)
        {
            _codeServices = codeServices;
            _presentationServices = presentationServices;
            _webApp = webApp;
            _contentProvider = contentProvider;
        }

        public bool Start(HostControl host)
        {
            LoadPresentations();            
            return _webApp.Start();
        }

        public bool Stop(HostControl host)
        {
            return _webApp.Stop();         
        }

        private async void LoadPresentations()
        {
            try
            {
                var presentations = await _contentProvider.GetAllContent(_presentationServices.Path, _presentationServices.Pattern);
                foreach (var presentation in presentations)
                {
                    await _presentationServices.AddItem(presentation);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
