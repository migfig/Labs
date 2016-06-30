using Castle.Windsor;
using Common.Generics;
using Common;
using System;
using System.IO;
using Topshelf;
using Trainer.Domain;
using System.Linq;

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
                var files = Directory.GetFiles(_presentationServices.Path, _presentationServices.Pattern);
                foreach (var file in files)
                {
                    var p = XmlHelper<Presentation>.Load(file);
                    if (p != null && p.Slide.Any())
                    {
                        await _presentationServices.AddItem(p);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
