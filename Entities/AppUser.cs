using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace QWiz.Entities;

public class AppUser : IdentityUser
{
    [Required] [MaxLength(100)] public string FullName { get; set; } = null!;

    [JsonIgnore] public override string ConcurrencyStamp => Guid.NewGuid().ToString();

    [JsonIgnore] public override string SecurityStamp => Guid.NewGuid().ToString();


    [JsonIgnore] public override string? PasswordHash { get; set; } = null!;
}