using QWiz.Entities;
using QWiz.Entities.Enum;

namespace QWiz.Helpers.Authentication;

public interface IAuthenticationService
{
    Task<AuthClaim> Login(LoginDto loginDto);
    Task<AppUser> Register(AppUser user, string password, Role role);
    Task<List<string>> GetRolesByUserId(string id);
    void ChangePasswordAsync(string email, string oldPassword, string confirmPassword);
    void ForgetPassword(string email, string domain);
    void ResetPassword(string email, string token, string newPassword);
    AppUser GetCurrentUser();
}