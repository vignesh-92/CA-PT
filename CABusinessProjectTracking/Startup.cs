using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusinessProjectTracking.Startup))]
namespace BusinessProjectTracking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);           
        }
    }
}
