using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScssMvcDemo.Startup))]
namespace ScssMvcDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
