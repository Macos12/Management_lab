using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ML.Startup))]
namespace ML
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
