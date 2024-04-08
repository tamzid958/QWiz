using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using QWiz.Entities.Enum;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class AppUserDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [Phone] public string PhoneNumber { get; set; } = null!;

    [Required] [PasswordPropertyText] public string Password { get; set; } = null!;

    [Required] public Role Role { get; set; } = Role.General;
}