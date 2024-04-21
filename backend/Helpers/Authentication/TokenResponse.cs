namespace QWiz.Helpers.Authentication;

public class TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}