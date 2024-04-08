using System.Data;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QWiz.Entities;

namespace QWiz.Databases;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly IDbConnection? _dbConnection;

    public AppDbContext(IDbConnection? dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser>? AppUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_dbConnection?.ToString() ?? throw new InvalidCredentialException());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}