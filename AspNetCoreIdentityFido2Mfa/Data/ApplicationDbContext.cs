﻿using System;
using System.Collections.Generic;
using System.Text;
using Fido2Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityFido2Mfa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FidoStoredCredential> FidoStoredCredential { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FidoStoredCredential>().HasKey(m => m.Id);

            base.OnModelCreating(builder);
        }
    }
}
