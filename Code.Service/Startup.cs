using Owin;
using System.Web.Http;

namespace Code.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new WindsorDependencyResolver(CodeService.Container.Kernel);
           
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AddApi",
                routeTemplate: "api/{controller}/add"
            );

            config.Routes.MapHttpRoute(
                name: "ViewApi",
                routeTemplate: "api/{controller}/view"
            );

            config.Routes.MapHttpRoute(
                name: "RemoveApi",
                routeTemplate: "api/{controller}/remove/{id}",
                defaults: new { id = string.Empty }
            );

            config.Routes.MapHttpRoute(
                name: "RemoveGenericApi",
                routeTemplate: "api/{controller}/remove/{propertyName}/{id}",
                defaults: new { propertyName = string.Empty, id = string.Empty }
            );

            builder.UseWebApi(config);
        }
    }
}
