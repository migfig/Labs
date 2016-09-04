using Castle.Windsor;
using Castle.Windsor.Installer;
using Code.Service;
using System.Web.Http;

namespace Trainer.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            config.DependencyResolver = new WindsorDependencyResolver(container.Kernel);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AddApi",
                routeTemplate: "api/{controller}/add"
            );

            config.Routes.MapHttpRoute(
                name: "AddCompleteApi",
                routeTemplate: "api/{controller}/add/complete"
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

            config.Routes.MapHttpRoute(
                name: "UpdateGenericApi",
                routeTemplate: "api/{controller}/update/{id}",
                defaults: new { id = string.Empty }
            );

            config.Routes.MapHttpRoute(
                name: "AuthenticateApi",
                routeTemplate: "api/{controller}/authenticate"
            );

            config.Routes.MapHttpRoute(
                name: "TokenAuthenticateApi",
                routeTemplate: "api/{controller}/token/{code}",
                defaults: new { code = string.Empty }
            );

            container.Resolve<CodeServiceWeb>().LoadPresentations();
        }
    }
}
