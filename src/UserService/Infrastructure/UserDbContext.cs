using Microsoft.EntityFrameworkCore;
using UserService.Commons.Models;

namespace UserService.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options):DbContext(options)
{
    public DbSet<UserDomain> Users => Set<UserDomain>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserDomain>().HasKey(x => x.UserId);
        builder.Entity<UserDomain>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<UserDomain>().HasIndex(x => x.Username).IsUnique();
        builder.Entity<UserDomain>().HasIndex(x => x.ExternalId).IsUnique();
        builder.Entity<UserDomain>().Property(x => x.PrivilegeUserDomain).HasConversion<int>();
        base.OnModelCreating(builder);
    }
}