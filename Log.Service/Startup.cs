using Owin;
using System.Web.Http;

namespace Log.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            //todo: need to assign IoC resolver
           
            config.Routes.MapHttpRoute(
                name: "TimeSpanApi",
                routeTemplate: "api/{controller}/{timeSpan}",
                defaults: new { timeSpan = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CountApi",
                routeTemplate: "api/{controller}/{count}/{level}",
                defaults: new { count = RouteParameter.Optional, level = RouteParameter.Optional }
            );

            builder.UseWebApi(config);
        }
    }
}
