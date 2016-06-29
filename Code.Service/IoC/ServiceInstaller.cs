using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Generics;
using domain = Trainer.Domain;

namespace Code.Service
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IGenericServices<domain.Presentation>>()
                    .ImplementedBy<GenericServices<domain.Presentation>>()
                    .DependsOn(Dependency.OnAppSettingsValue("maxItems"), Dependency.OnAppSettingsValue("path"), Dependency.OnAppSettingsValue("pattern"))
                    .LifestyleSingleton(),

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

                Component.For<PresentationsController>()
                    .LifestyleTransient(),

                Component.For<CodeService>()
                    .LifestyleSingleton()
                );
        }
    }
}
