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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{timeSpan}",
                defaults: new { timeSpan = RouteParameter.Optional }
            );

            builder.UseWebApi(config);
        }
    }
}
