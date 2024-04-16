using QWiz.Databases;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Wrapper;

public class RepositoryWrapper(
    AppDbContext context,
    IUriService uriService,
    IAuthenticationService authenticationService) : IRepositoryWrapper
{
    private IApprovalLogRepository? _approvalLogRepository;
    private IApproverRepository? _approverRepository;
    private IAppUserRepository? _appUserRepository;
    private ICategoryRepository? _categoryRepository;
    private IQuestionRepository? _questionRepository;

    public IAppUserRepository AppUser =>
        _appUserRepository ??= new AppUserRepository(context, uriService, authenticationService);

    public IApprovalLogRepository ApprovalLog => _approvalLogRepository ??=
        new ApprovalLogRepository(context, uriService, authenticationService);

    public IApproverRepository Approver =>
        _approverRepository ??= new ApproverRepository(context, uriService, authenticationService);

    public ICategoryRepository Category =>
        _categoryRepository ??= new CategoryRepository(context, uriService, authenticationService);

    public IQuestionRepository Question =>
        _questionRepository ??= new QuestionRepository(context, uriService, authenticationService);
}