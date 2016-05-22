using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Log.Common;
using Log.Provider.Default;

namespace Log.Service
{
    public class LogInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILogProvider>()
                    .ImplementedBy<CustomFileProvider>()
                    .DependsOn(Dependency.OnAppSettingsValue("path"),
                        Dependency.OnAppSettingsValue("name"))
                    .LifestyleSingleton(),

                Component.For<ILogServices>()
                    .ImplementedBy<LogServices>()
                    .LifestyleSingleton(),

                Component.For<IStartable>()
                    .ImplementedBy<WebApiApp>()
                    .DependsOn(Dependency.OnAppSettingsValue("port"))
                    .LifestyleTransient(),

                Component.For<LogController>()
                    .LifestyleTransient(),

                Component.For<LogService>()
                    .LifestyleSingleton()
                );
        }
    }
}
