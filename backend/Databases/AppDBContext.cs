using System.Data;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QWiz.Entities;
using QWiz.Entities.Enum;

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
    public virtual DbSet<Reviewer>? Reviewers { get; set; }
    public virtual DbSet<Category>? Categories { get; set; }
    public virtual DbSet<Question>? Questions { get; set; }
    public virtual DbSet<ReviewLog>? ReviewLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_dbConnection?.ToString() ?? throw new InvalidCredentialException());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        const string adminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
        const string roleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = Role.Admin.ToString(),
            NormalizedName = Role.Admin.ToString().ToUpper(),
            Id = roleId,
            ConcurrencyStamp = roleId
        }, new IdentityRole
        {
            Name = Role.Reviewer.ToString(),
            NormalizedName = Role.Reviewer.ToString().ToUpper()
        }, new IdentityRole
        {
            Name = Role.QuestionSetter.ToString(),
            NormalizedName = Role.QuestionSetter.ToString().ToUpper()
        });

        var appUser = new AppUser
        {
            Id = adminId,
            Email = "tamjidahmed958@gmail.com",
            EmailConfirmed = true,
            FullName = "Tamzid Ahmed",
            UserName = "tamzid",
            NormalizedUserName = "TAMZID"
        };

        var ph = new PasswordHasher<AppUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, "tamzid");

        //seed user
        builder.Entity<AppUser>().HasData(appUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleId,
            UserId = adminId
        });
    }
}