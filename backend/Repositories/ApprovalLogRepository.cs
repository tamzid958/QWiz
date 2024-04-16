using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class ApprovalLogRepository(
    AppDbContext context,
    IUriService uriService,
    IAuthenticationService authenticationService)
    : BaseRepository<ApprovalLog>(context, uriService, authenticationService), IApprovalLogRepository;