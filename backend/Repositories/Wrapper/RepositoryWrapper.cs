using QWiz.Databases;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Wrapper;

public class RepositoryWrapper(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor) : IRepositoryWrapper
{
    private IApprovalLogRepository? _approvalLogRepository;
    private IApproverRepository? _approverRepository;
    private IAppUserRepository? _appUserRepository;
    private ICategoryRepository? _categoryRepository;
    private IQuestionRepository? _questionRepository;

    public IAppUserRepository AppUser =>
        _appUserRepository ??= new AppUserRepository(context, uriService, httpContextAccessor);

    public IApprovalLogRepository ApprovalLog => _approvalLogRepository ??=
        new ApprovalLogRepository(context, uriService, httpContextAccessor);

    public IApproverRepository Approver =>
        _approverRepository ??= new ApproverRepository(context, uriService, httpContextAccessor);

    public ICategoryRepository Category =>
        _categoryRepository ??= new CategoryRepository(context, uriService, httpContextAccessor);

    public IQuestionRepository Question =>
        _questionRepository ??= new QuestionRepository(context, uriService, httpContextAccessor);
}