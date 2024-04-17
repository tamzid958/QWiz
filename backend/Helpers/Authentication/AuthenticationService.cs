using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QWiz.Entities;
using QWiz.Entities.Enum;
using QWiz.Repositories.Wrapper;

namespace QWiz.Helpers.Authentication;

public class AuthenticationService(
    IConfiguration configuration,
    UserManager<AppUser> userManager,
    IRepositoryWrapper repositoryWrapper,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task<AuthClaim> Login(LoginDto loginDto)
    {
        try
        {
            var user = await userManager.FindByNameAsync(loginDto.Username) ??
                       await userManager.FindByEmailAsync(loginDto.Username) ??
                       repositoryWrapper.AppUser.GetFirstBy(o => o.PhoneNumber == loginDto.Username);

            if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new UnauthorizedAccessException("invalid email and password");


            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

            var token = new JwtSecurityToken(
                configuration["JWT:ValidIssuer"],
                configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );


            return new AuthClaim
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                AppUser = user
            };
        }
        catch (System.Exception e)
        {
            throw new UnauthorizedAccessException(e.ToString());
        }
    }

    public async Task<AppUser> Register(AppUser user, string password, Role role)
    {
        try
        {
            if (await userManager.FindByEmailAsync(user.Email!) != null)
                throw new DuplicateNameException("Email already registered with different user");
            if (repositoryWrapper.AppUser.Any(o => o.PhoneNumber == user.PhoneNumber))
                throw new DuplicateNameException("Phone Number already registered with different user");
            var userResult = await userManager.CreateAsync(user, password);
            if (!userResult.Succeeded)
                throw new InvalidDataException("user registration failed");
            var newUser = await userManager.FindByEmailAsync(user.Email!);
            var roleResult = await userManager.AddToRoleAsync(newUser!, Enum.GetName(role)!);
            if (!roleResult.Succeeded)
                throw new InvalidOperationException("user role assignment failed");
            return newUser!;
        }
        catch (System.Exception e)
        {
            throw new InvalidDataException(e.ToString());
        }
    }

    public async Task<List<string>> GetRolesByUserId(string id)
    {
        return [.. await userManager.GetRolesAsync((await userManager.FindByIdAsync(id))!)];
    }

    public async void ChangePasswordAsync(string email, string oldPassword, string confirmPassword)
    {
        await userManager.ChangePasswordAsync(
            (await userManager.FindByEmailAsync(email))!,
            oldPassword,
            confirmPassword
        );
    }

    public async void ForgetPassword(string email, string domain)
    {
        var user = await userManager.FindByEmailAsync(email);
        var token = await userManager.GeneratePasswordResetTokenAsync(user!);
        var encodedEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(user!.Email!));


        // send email to the user email
    }

    public async void ResetPassword(string email, string token, string newPassword)
    {
        await userManager.ResetPasswordAsync(
            (await userManager.FindByEmailAsync(email))!,
            token,
            newPassword
        );
    }

    public AppUser GetCurrentUser()
    {
        var userIdentity = httpContextAccessor.HttpContext!.User.Identity;
        if (userIdentity != null) return repositoryWrapper.AppUser.GetFirstBy(o => o.UserName == userIdentity.Name);

        throw new UnauthorizedAccessException("User not authenticated or identity not found");
    }
}