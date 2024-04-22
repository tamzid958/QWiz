using QWiz.Databases;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Wrapper;

public class RepositoryWrapper(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor) : IRepositoryWrapper
{
    private IReviewLogRepository? _approvalLogRepository;
    private IReviewerRepository? _approverRepository;
    private IAppUserRepository? _appUserRepository;
    private ICategoryRepository? _categoryRepository;
    private IQuestionRepository? _questionRepository;

    public IAppUserRepository AppUser =>
        _appUserRepository ??= new AppUserRepository(context, uriService, httpContextAccessor);

    public IReviewLogRepository ReviewLog => _approvalLogRepository ??=
        new ReviewLogRepository(context, uriService, httpContextAccessor);

    public IReviewerRepository Reviewer =>
        _approverRepository ??= new ReviewerRepository(context, uriService, httpContextAccessor);

    public ICategoryRepository Category =>
        _categoryRepository ??= new CategoryRepository(context, uriService, httpContextAccessor);

    public IQuestionRepository Question =>
        _questionRepository ??= new QuestionRepository(context, uriService, httpContextAccessor);
}