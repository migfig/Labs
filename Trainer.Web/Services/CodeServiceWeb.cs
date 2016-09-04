using Castle.Windsor;
using Common.Generics;
using System;
using Trainer.Domain;
using Common.Controllers;

namespace Code.Service
{
    public class CodeServiceWeb
    {
        public static IWindsorContainer Container { get; set; }
        private readonly ICodeServices _codeServices;
        private readonly IGenericServices<Presentation> _presentationServices;
        private readonly IContentProvider<Presentation> _contentProvider;

        public CodeServiceWeb(ICodeServices codeServices, IGenericServices<Presentation> presentationServices, IContentProvider<Presentation> contentProvider)
        {
            _codeServices = codeServices;
            _presentationServices = presentationServices;
            _contentProvider = contentProvider;
        }

        public async void LoadPresentations()
        {
            try
            {
                var presentations = await _contentProvider.GetAllContent(_presentationServices.Path, _presentationServices.Pattern);
                foreach (var presentation in presentations)
                {
                    await _presentationServices.AddItem(presentation);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
