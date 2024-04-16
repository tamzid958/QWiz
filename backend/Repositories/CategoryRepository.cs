using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class CategoryRepository(
    AppDbContext context,
    IUriService uriService,
    IAuthenticationService authenticationService)
    : BaseRepository<Category>(context, uriService, authenticationService), ICategoryRepository;