using Castle.Windsor;
using Castle.Windsor.Installer;
using Common.Generics;
using Topshelf;
using Trainer.Domain;

namespace Code.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            CodeService.Container = new WindsorContainer();
            CodeService.Container.Install(FromAssembly.This());

            var codeServices = CodeService.Container.Resolve<ICodeServices>();
            var presentationServices = CodeService.Container.Resolve<IGenericServices<Presentation>>();
            var webApp = CodeService.Container.Resolve<IServiceable>();

            var hostObj = HostFactory.New(x =>
            {
                x.Service<CodeService>(s =>
                {
                    s.ConstructUsing(name => new CodeService(codeServices, presentationServices, webApp));
                    s.WhenStarted((ls, host) => ls.Start(host));
                    s.WhenStopped((ls, host) => ls.Stop(host));                    
                });

                x.RunAsLocalSystem().SetDescription("Code services console");
                x.SetDisplayName("Code Service");
                x.SetServiceName("CodeService");
                x.StartAutomatically();
            });

            hostObj.Run();
        }
    }
}
