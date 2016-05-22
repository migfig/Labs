using Castle.Windsor;
using Castle.Windsor.Installer;
using Topshelf;

namespace Log.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            var logServices = container.Resolve<ILogServices>();

            var hostObj = HostFactory.New(x =>
            {
                x.Service<LogService>(s =>
                {
                    s.ConstructUsing(name => new LogService(logServices));
                    s.WhenStarted((ls, host) => ls.Start(host));
                    s.WhenStopped((ls, host) => ls.Stop(host));                    
                });

                x.RunAsLocalSystem();
                x.SetDescription("Log services console");
                x.SetDisplayName("Log Service");
                x.SetServiceName("LogService");
                x.StartAutomatically();
            });

            hostObj.Run();
        }
    }
}
