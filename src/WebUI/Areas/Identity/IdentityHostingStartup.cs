using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Zenbot.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace Zenbot.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}