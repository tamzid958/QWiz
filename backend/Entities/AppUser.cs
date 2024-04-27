using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace QWiz.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class AppUser : IdentityUser
{
    [Required] [MaxLength(100)] public required string FullName { get; set; }

    [Required] [EmailAddress] public override string Email { get; set; }

    [Required] [Phone] public override string PhoneNumber { get; set; }

    [JsonIgnore] public override string ConcurrencyStamp => Guid.NewGuid().ToString();

    [JsonIgnore] public override string SecurityStamp => Guid.NewGuid().ToString();


    [JsonIgnore] public override string? PasswordHash { get; set; } = null!;

    [JsonIgnore] [MaxLength(400)] public string? RefreshToken { get; set; }

    [JsonIgnore] public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.Now;

    public virtual ICollection<ApplicationUserRole> UserRoles { get; } = new List<ApplicationUserRole>();
}