[assembly: HostingStartup(typeof(AspNetCoreIdentityFido2Passwordless.Areas.Identity.IdentityHostingStartup))]
namespace AspNetCoreIdentityFido2Passwordless.Areas.Identity
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