using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ihff.Startup))]
namespace ihff
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
