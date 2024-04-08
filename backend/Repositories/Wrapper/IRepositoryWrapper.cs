namespace QWiz.Repositories.Wrapper;

public interface IRepositoryWrapper
{
    IAppUserRepository AppUser { get; }
    IApprovalLogRepository ApprovalLog { get; }
    IApproverRepository Approver { get; }
    ICategoryRepository Category { get; }
    IQuestionRepository Question { get; }
}