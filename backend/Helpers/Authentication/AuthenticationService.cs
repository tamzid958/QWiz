using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using QWiz.Entities;
using QWiz.Entities.Enum;
using QWiz.Helpers.Authentication.TokenService;
using QWiz.Repositories.Wrapper;

namespace QWiz.Helpers.Authentication;

public class AuthenticationService(
    UserManager<AppUser> userManager,
    IRepositoryWrapper repositoryWrapper,
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task<AuthClaim> Login(LoginDto loginDto)
    {
        try
        {
            var user = repositoryWrapper
                .AppUser
                .GetFirstBy(o => o.UserName == loginDto.Username && !o.LockoutEnabled,
                    o => o.UserRoles.Select(x => x.Role)
                );

            if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new UnauthorizedAccessException("invalid email and password");


            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(
                userRoles.Select(userRole => new Claim(
                    ClaimTypes.Role,
                    userRole
                ))
            );
            var refreshToken = tokenService.GenerateRefreshToken();


            var updateUser = repositoryWrapper.AppUser.GetById(user.Id);
            updateUser.RefreshToken = refreshToken;
            updateUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await userManager.UpdateAsync(updateUser);

            var tokenString = tokenService.GenerateAccessToken(authClaims);

            return new AuthClaim
            {
                Token = tokenString,
                RefreshToken = refreshToken,
                AppUser = user,
                Roles = user.UserRoles.Select(o => o.Role.Name).ToList()!
            };
        }
        catch (System.Exception e)
        {
            throw new UnauthorizedAccessException(e.ToString());
        }
    }

    public AuthClaim RefreshToken(TokenModel tokenModel)
    {
        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        if (accessToken == null || refreshToken == null)
            throw new AuthenticationException("empty accessToken or refresh token");
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal.Identity == null) throw new AuthenticationFailureException("Invalid user");

        var username = principal.Identity.Name;
        var user = repositoryWrapper.AppUser.GetFirstBy(
            o => o.UserName == username,
            o => o.UserRoles.Select(x => x.Role)
        );
        if (user == null) throw new AuthenticationFailureException("Invalid user");

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new InvalidCredentialException("Invalid client request");

        var tokenString = tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        var updateUser = repositoryWrapper.AppUser.GetById(user.Id);
        updateUser.RefreshToken = newRefreshToken;
        repositoryWrapper.AppUser.Update(updateUser);

        return new AuthClaim
        {
            Token = tokenString,
            RefreshToken = refreshToken,
            AppUser = user,
            Roles = user.UserRoles.Select(o => o.Role.Name).ToList()!
        };
    }


    public async Task<AppUser> Register(AppUser user, string password, List<Role> roles)
    {
        try
        {
            if (await userManager.FindByEmailAsync(user.Email) != null)
                throw new DuplicateNameException("Email already registered with different user");
            if (repositoryWrapper.AppUser.Any(o => o.PhoneNumber == user.PhoneNumber))
                throw new DuplicateNameException("Phone Number already registered with different user");
            var userResult = await userManager.CreateAsync(user, password);
            if (!userResult.Succeeded)
                throw new InvalidDataException("user registration failed");
            var newUser = await userManager.FindByEmailAsync(user.Email);
            var roleResult = await userManager.AddToRolesAsync(newUser!, roles.ConvertAll(input => input.ToString()));
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

    public async Task ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user == null) throw new UnauthorizedAccessException();
        await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
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
        if (userIdentity != null)
            return repositoryWrapper.AppUser.GetFirstBy(
                o => o.UserName == userIdentity.Name,
                o => o.UserRoles.Select(x => x.Role)
            );

        throw new UnauthorizedAccessException("User not authenticated or identity not found");
    }

    public AppUser LockAccount(string id)
    {
        var user = repositoryWrapper.AppUser.GetById(id);
        user.LockoutEnabled = !user.LockoutEnabled;
        if (user.LockoutEnabled) user.RefreshToken = null;
        return repositoryWrapper.AppUser.Update(user);
    }

    public AppUser CurrentUserInWhichRole(out bool isAdmin, out bool isReviewer, out bool isQuestionSetter)
    {
        var currentUser = GetCurrentUser();

        var roles = GetRolesByUserId(currentUser.Id).Result;

        isAdmin = roles.Contains("Admin");
        isReviewer = roles.Contains("Reviewer");
        isQuestionSetter = roles.Contains("QuestionSetter");
        return currentUser;
    }
}