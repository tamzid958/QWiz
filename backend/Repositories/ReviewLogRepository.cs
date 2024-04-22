using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class ReviewLogRepository(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor)
    : BaseRepository<ReviewLog>(context, uriService, httpContextAccessor), IReviewLogRepository;