using System.Data;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QWiz.Entities;
using QWiz.Entities.Enum;

namespace QWiz.Databases;

public class AppDbContext : IdentityDbContext<AppUser, ApplicationRole, string, IdentityUserClaim<string>
    , ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationUserRole>()
            .HasOne(e => e.User)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<ApplicationUserRole>()
            .HasOne(e => e.Role)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.RoleId);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        var adminId = Guid.NewGuid().ToString();
        var roleId = Guid.NewGuid().ToString();

        builder.Entity<ApplicationRole>().HasData(new ApplicationRole
        {
            Name = Role.Admin.ToString(),
            NormalizedName = Role.Admin.ToString().ToUpper(),
            Id = roleId,
            ConcurrencyStamp = roleId
        }, new ApplicationRole
        {
            Name = Role.Reviewer.ToString(),
            NormalizedName = Role.Reviewer.ToString().ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationRole
        {
            Name = Role.QuestionSetter.ToString(),
            NormalizedName = Role.QuestionSetter.ToString().ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
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

        builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
        {
            RoleId = roleId,
            UserId = adminId
        });
    }
}