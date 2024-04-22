namespace QWiz.Repositories.Wrapper;

public interface IRepositoryWrapper
{
    IAppUserRepository AppUser { get; }
    IReviewLogRepository ReviewLog { get; }
    IReviewerRepository Reviewer { get; }
    ICategoryRepository Category { get; }
    IQuestionRepository Question { get; }
}