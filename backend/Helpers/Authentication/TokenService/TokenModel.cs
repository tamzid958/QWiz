namespace QWiz.Helpers.Authentication.TokenService;

public class TokenModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}