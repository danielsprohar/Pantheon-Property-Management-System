using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Pantheon.Identity.Areas.Identity.IdentityHostingStartup))]

namespace Pantheon.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}