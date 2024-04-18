using QWiz.Entities;

namespace QWiz.Helpers.Authentication;

public class AuthClaim
{
    public string Token { set; get; } = null!;
    public DateTime Expiration { set; get; }

    public AppUser AppUser { set; get; } = null!;

    public List<string> Roles { set; get; } = null!;
}