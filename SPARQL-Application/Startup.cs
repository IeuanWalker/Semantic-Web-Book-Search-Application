using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPARQL_Application.Startup))]
namespace SPARQL_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
