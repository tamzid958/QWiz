using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class CategoryRepository(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor)
    : BaseRepository<Category>(context, uriService, httpContextAccessor), ICategoryRepository;