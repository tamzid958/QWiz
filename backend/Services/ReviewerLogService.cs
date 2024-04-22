using AutoMapper;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.HttpQueries;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ReviewerLogService(
    IRepositoryWrapper repositoryWrapper,
    AuthenticationService authenticationService,
    IMapper mapper
)
{
    public List<ReviewLog> Get(HttpRequest request, ReviewLogQueries reviewLogQueries)
    {
        return repositoryWrapper.ReviewLog.GetAll(
            log => log.QuestionId == reviewLogQueries.QuestionId
        );
    }

    public ReviewLog Create(ReviewLogDto reviewLog)
    {
        var recentApprovalLog = repositoryWrapper.ReviewLog.Insert(mapper.Map<ReviewLog>(reviewLog));

        UpdateQuestionBasedApprovalLog(recentApprovalLog);

        return recentApprovalLog;
    }

    private void UpdateQuestionBasedApprovalLog(ReviewLog reviewLog)
    {
        var question = repositoryWrapper.Question.GetFirstBy(o => o.Id == reviewLog.QuestionId);
        var approvers = repositoryWrapper.Reviewer.GetAll(
            o => o.Category == question.Category &&
                 o.AppUser != authenticationService.GetCurrentUser()
        );

        var approvalLogs = repositoryWrapper.ReviewLog.GetAll(
            o => o.QuestionId == reviewLog.QuestionId
        );

        var isAllApproverReviewed =
            approvers.All(
                approver => approvalLogs
                    .Select(x => x.CreatedBy)
                    .Contains(approver.AppUser)
            );

        if (!isAllApproverReviewed) return;
        question.IsReadyForAddingQuestionBank = true;
        repositoryWrapper.Question.Update(question);
    }

    public ReviewLog Update(long id, ReviewLogDto reviewLog)
    {
        reviewLog.Id = id;
        return repositoryWrapper.ReviewLog.Update(mapper.Map<ReviewLog>(reviewLog));
    }
}