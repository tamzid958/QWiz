using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class ApprovalLogRepository(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor)
    : BaseRepository<ApprovalLog>(context, uriService, httpContextAccessor), IApprovalLogRepository;