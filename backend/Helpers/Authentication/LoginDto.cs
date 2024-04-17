using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.Authentication;

public class LoginDto
{
    [Required] public string Username { set; get; } = null!;

    [Required] [PasswordPropertyText] public string Password { set; get; } = null!;
}