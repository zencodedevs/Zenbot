using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ZenAchitecture.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace ZenAchitecture.WebUI.Areas.Identity
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