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
        var reviewers = repositoryWrapper.Reviewer.GetAll(
                o => o.CategoryId == question.CategoryId
            ).Where(reviewer => reviewer.AppUserId != question.CreatedById)
            .ToList();

        var reviewLogs = repositoryWrapper.ReviewLog.GetAll(
            o => o.QuestionId == reviewLog.QuestionId
        );

        var isAllReviewerReviewed = reviewers.Count == reviewLogs.Count;

        if (!isAllReviewerReviewed) return;
        question.IsReadyForAddingQuestionBank = true;
        repositoryWrapper.Question.Update(question);
    }

    public ReviewLog Update(long id, ReviewLogDto reviewLog)
    {
        reviewLog.Id = id;

        var currentUser = authenticationService.CurrentUserInWhichRole(
            out _,
            out var isReviewer,
            out var _
        );

        if (!isReviewer) return repositoryWrapper.ReviewLog.Update(mapper.Map<ReviewLog>(reviewLog));

        var prevReviewLog = repositoryWrapper.ReviewLog.GetById(id);
        if (prevReviewLog.CreatedBy != currentUser) throw new UnauthorizedAccessException();

        return repositoryWrapper.ReviewLog.Update(mapper.Map<ReviewLog>(reviewLog));
    }
}