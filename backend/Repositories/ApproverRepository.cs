using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class ApproverRepository(AppDbContext context, IUriService uriService)
    : BaseRepository<Approver>(context, uriService), IApproverRepository;