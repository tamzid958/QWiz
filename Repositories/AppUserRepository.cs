using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class AppUserRepository(AppDbContext context, IUriService uriService)
    : BaseRepository<AppUser>(context, uriService), IAppUserRepository;