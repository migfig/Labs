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
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            builder.UseWebApi(config);
        }
    }
}
