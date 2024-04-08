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
                string.IsNullOrEmpty(appUserQueries.FullName) ||
                (user.FullName.Contains(appUserQueries.FullName, StringComparison.CurrentCultureIgnoreCase) &&
                 (string.IsNullOrEmpty(appUserQueries.UserName) ||
                  user.UserName!.Contains(appUserQueries.UserName, StringComparison.CurrentCultureIgnoreCase)) &&
                 (string.IsNullOrEmpty(appUserQueries.PhoneNumber) ||
                  user.PhoneNumber!.Contains(appUserQueries.PhoneNumber!, StringComparison.CurrentCultureIgnoreCase)) &&
                 (string.IsNullOrEmpty(appUserQueries.Email) ||
                  user.Email!.Contains(appUserQueries.Email, StringComparison.CurrentCultureIgnoreCase))));
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