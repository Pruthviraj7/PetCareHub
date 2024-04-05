using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetCareHub.Startup))]
namespace PetCareHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
