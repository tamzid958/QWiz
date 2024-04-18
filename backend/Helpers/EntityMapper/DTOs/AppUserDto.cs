using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using QWiz.Entities.Enum;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class AppUserDto
{
    [Required] public string FullName { get; set; } = null!;
    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [Phone] public string PhoneNumber { get; set; } = null!;

    [Required] [PasswordPropertyText] public string Password { get; set; } = null!;

    [Required] public List<Role> Roles { get; set; } = [Role.QuestionSetter];
}