using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InstepTechnologies.Startup))]
namespace InstepTechnologies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
