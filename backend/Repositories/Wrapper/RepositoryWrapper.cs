using QWiz.Databases;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Wrapper;

public class RepositoryWrapper(AppDbContext context, IUriService uriService) : IRepositoryWrapper
{
    private IAppUserRepository? _appUserRepository;
    private IApprovalLogRepository? _approvalLogRepository;
    private IApproverRepository? _approverRepository;
    private ICategoryRepository? _categoryRepository;
    private IQuestionRepository? _questionRepository;
    
    public IAppUserRepository AppUser => _appUserRepository ??= new AppUserRepository(context, uriService);
    public IApprovalLogRepository ApprovalLog => _approvalLogRepository ??= new ApprovalLogRepository(context, uriService);
    public IApproverRepository Approver => _approverRepository ??= new ApproverRepository(context, uriService);
    public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(context, uriService);
    public IQuestionRepository Question => _questionRepository ??= new QuestionRepository(context, uriService);
}