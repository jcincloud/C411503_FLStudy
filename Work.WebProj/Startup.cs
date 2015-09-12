using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotWeb.AppStart.Startup))]
namespace DotWeb.AppStart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
