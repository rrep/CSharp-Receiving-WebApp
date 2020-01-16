using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eRaceWebApp.Startup))]
namespace eRaceWebApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
