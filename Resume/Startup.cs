using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Resume.Startup))]
namespace Resume
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
