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

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of FIDO2 stored credentials.
    /// </summary>
    public virtual DbSet<FidoStoredCredential> FidoStoredCredential { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FidoStoredCredential>().HasKey(m => m.Id);

        base.OnModelCreating(builder);
    }
}
