using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.HttpQueries;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class AppUserService(IRepositoryWrapper repositoryWrapper)
{
    public PagedResponse<List<AppUser>> Get(PaginationFilter paginationFilter, HttpRequest request,
        AppUserQueries appUserQueries)
    {
        return repositoryWrapper.AppUser.GetAll(paginationFilter, request,
            user =>
                (string.IsNullOrEmpty(appUserQueries.FullName) ||
                 user.FullName.ToLower().StartsWith(appUserQueries.FullName.ToLower())) &&
                (string.IsNullOrEmpty(appUserQueries.UserName) ||
                 user.UserName!.ToLower().StartsWith(appUserQueries.UserName.ToLower())) &&
                (string.IsNullOrEmpty(appUserQueries.PhoneNumber) ||
                 user.PhoneNumber!.StartsWith(appUserQueries.PhoneNumber)) &&
                (string.IsNullOrEmpty(appUserQueries.Email) ||
                 user.Email!.ToLower().StartsWith(appUserQueries.Email.ToLower())) &&
                (appUserQueries.Roles == null ||
                 user.UserRoles.Any(ur => appUserQueries.Roles.Any(qr => ur.Role.Name == qr))
                ),
            o => o.UserRoles.Select(x => x.Role)
        );
    }

    public AppUser GetById(string id)
    {
        return repositoryWrapper.AppUser.GetById(id);
    }

    public AppUser Update(string id, UpdateAppUserDto userDto)
    {
        var user = repositoryWrapper.AppUser.GetById(id);

        user.FullName = userDto.FullName;
        user.PhoneNumber = userDto.PhoneNumber;

        return repositoryWrapper.AppUser.Update(user);
    }
}