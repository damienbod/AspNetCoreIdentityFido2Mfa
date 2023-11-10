using Fido2Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityFido2Mfa.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FidoCredential> FidoCredentials => Set<FidoCredential>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FidoCredential>().HasKey(m => m.Id);

        base.OnModelCreating(builder);
    }
}