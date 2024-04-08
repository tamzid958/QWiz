using QWiz.Databases;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Wrapper;

public class RepositoryWrapper(AppDbContext context, IUriService uriService) : IRepositoryWrapper
{
    private IAppUserRepository? _appUserRepository;

    public IAppUserRepository AppUser => _appUserRepository ??= new AppUserRepository(context, uriService);
}