using Microsoft.Owin;
using Owin;
using WebSite.Util;

[assembly: OwinStartupAttribute(typeof(WebSite.Startup))]
namespace WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutofacConfig.ConfigureContainer();
            ConfigureAuth(app);
        }
    }
}
