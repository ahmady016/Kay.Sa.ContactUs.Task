using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
  public class ContactUsContext : DbContext
  {
    public ContactUsContext(DbContextOptions options) : base(options) { }

    #region DbSets
    public DbSet<Inquiry> Inquiries { get; set; }

    #endregion

    #region Model Builder [Fluent API Configurations]
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Inquiry>()
        .HasIndex(u => u.Email)
        .IsUnique();
    }
    #endregion

  }
}
