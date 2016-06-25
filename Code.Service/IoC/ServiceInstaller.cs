using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Code.Service
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ICodeServices>()
                    .ImplementedBy<CodeServices>()
                    .DependsOn(Dependency.OnAppSettingsValue("maxItems"))
                    .LifestyleSingleton(),

                Component.For<IServiceable>()
                    .ImplementedBy<WebApiApp>()
                    .DependsOn(Dependency.OnAppSettingsValue("port"))
                    .LifestyleTransient(),

                Component.For<ComponentsController>()
                    .LifestyleTransient(),

                Component.For<CodeService>()
                    .LifestyleSingleton()
                );
        }
    }
}
