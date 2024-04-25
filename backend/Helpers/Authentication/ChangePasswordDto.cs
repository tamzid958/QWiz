namespace QWiz.Helpers.Authentication;

public class ChangePasswordDto
{
    public string UserName { set; get; }
    public string OldPassword { set; get; }
    public string NewPassword { set; get; }
}