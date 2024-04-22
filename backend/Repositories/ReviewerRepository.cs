using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class ReviewerRepository(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor)
    : BaseRepository<Reviewer>(context, uriService, httpContextAccessor), IReviewerRepository;