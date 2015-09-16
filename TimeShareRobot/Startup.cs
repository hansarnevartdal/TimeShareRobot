using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TimeShareRobot.Server.Startup))]
namespace TimeShareRobot.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
