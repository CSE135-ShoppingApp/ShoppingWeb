using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shoppa.Startup))]
namespace Shoppa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
