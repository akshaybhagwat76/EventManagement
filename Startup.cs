using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MiidWeb.Startup))]
namespace MiidWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
