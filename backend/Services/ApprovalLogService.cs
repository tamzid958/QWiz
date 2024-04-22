using AutoMapper;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.HttpQueries;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ApprovalLogService(
    IRepositoryWrapper repositoryWrapper,
    AuthenticationService authenticationService,
    IMapper mapper
)
{
    public List<ApprovalLog> Get(HttpRequest request, ApprovalLogQueries approvalLogQueries)
    {
        return repositoryWrapper.ApprovalLog.GetAll(
            log => log.QuestionId == approvalLogQueries.QuestionId
        );
    }

    public ApprovalLog Create(ApprovalLogDto approvalLog)
    {
        var recentApprovalLog = repositoryWrapper.ApprovalLog.Insert(mapper.Map<ApprovalLog>(approvalLog));

        UpdateQuestionBasedApprovalLog(recentApprovalLog);

        return recentApprovalLog;
    }

    private void UpdateQuestionBasedApprovalLog(ApprovalLog approvalLog)
    {
        var question = repositoryWrapper.Question.GetFirstBy(o => o.Id == approvalLog.QuestionId);
        var approvers = repositoryWrapper.Approver.GetAll(
            o => o.Category == question.Category &&
                 o.AppUser != authenticationService.GetCurrentUser()
        );

        var approvalLogs = repositoryWrapper.ApprovalLog.GetAll(
            o => o.QuestionId == approvalLog.QuestionId
        );

        var isAllApproverReviewed =
            approvers.All(
                approver => approvalLogs
                    .Select(x => x.CreatedBy)
                    .Contains(approver.AppUser)
            );

        if (!isAllApproverReviewed) return;
        question.IsAddedToQuestionBank = approvalLogs.All(o => o.IsApproved);
        repositoryWrapper.Question.Update(question);
    }

    public ApprovalLog Update(long id, ApprovalLogDto approvalLog)
    {
        approvalLog.Id = id;
        return repositoryWrapper.ApprovalLog.Update(mapper.Map<ApprovalLog>(approvalLog));
    }
}