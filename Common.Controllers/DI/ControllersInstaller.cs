using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Data;
using Common.Data.Models;
using Common.Data.Repositories;
using System.Web.Http;

namespace Common.Controllers
{
    public sealed class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRepository<Customer>>()
                    .ImplementedBy<CustomerRepository>()
                    .LifestyleScoped(),

                Component.For<IRepository<Product>>()
                    .ImplementedBy<ProductRepository>()
                    .LifestyleScoped(),

                Component.For<IRepository<Category>>()
                    .ImplementedBy<CategoryRepository>()
                    .LifestyleScoped(),

                Classes.FromThisAssembly()
                    .BasedOn<ApiController>()
                    .LifestyleScoped()
                );
        }
    }
}
