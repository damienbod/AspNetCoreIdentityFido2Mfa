[assembly: HostingStartup(typeof(AspNetCoreIdentityFido2Mfa.Areas.Identity.IdentityHostingStartup))]
namespace AspNetCoreIdentityFido2Mfa.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}