using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class AppUserQueries
{
    [FromQuery(Name = "userName")] public string? UserName { set; get; }
    [FromQuery(Name = "fullName")] public string? FullName { set; get; }
    [FromQuery(Name = "email")] public string? Email { set; get; }
    [FromQuery(Name = "phoneNumber")] public string? PhoneNumber { set; get; }

    [FromQuery(Name = "roles")] public List<string>? Roles { set; get; }
}