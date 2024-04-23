using System.Data;
using AutoMapper;
using QWiz.Entities;
using QWiz.Entities.Enum;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.HttpQueries;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class QuestionService(
    IRepositoryWrapper repositoryWrapper,
    AuthenticationService authenticationService,
    IMapper mapper)

{
    public PagedResponse<List<Question>> Get(HttpRequest request, PaginationFilter paginationFilter,
        QuestionQueries questionQueries)
    {
        var currentUser = authenticationService.CurrentUserInWhichRole(
            out var isAdmin,
            out var isReviewer,
            out var isQuestionSetter
        );

        return repositoryWrapper.Question.GetAll(paginationFilter, request,
            question => (questionQueries.CategoryId == null || question.CategoryId == questionQueries.CategoryId) &&
                        (
                            questionQueries.IsReadyForAddingQuestionBank == null ||
                            question.IsReadyForAddingQuestionBank == questionQueries.IsReadyForAddingQuestionBank
                        ) &&
                        (
                            questionQueries.IsAddedToQuestionBank == null ||
                            question.IsAddedToQuestionBank == questionQueries.IsAddedToQuestionBank
                        ) &&
                        (
                            isQuestionSetter
                                ? question.CreatedById == currentUser.Id
                                : isAdmin ||
                                  (
                                      isReviewer && question.Category.Reviewers.Any(reviewer =>
                                          reviewer.AppUserId == currentUser.Id)
                                  )
                        ) &&
                        (
                            questionQueries.ReviewMode == null ||
                            (
                                isReviewer && questionQueries.ReviewMode == ReviewMode.Reviewed
                                    ? question.ReviewLogs.Any(log => log.CreatedById == currentUser.Id)
                                    : isReviewer && questionQueries.ReviewMode == ReviewMode.UnderReview
                                        ? question.ReviewLogs.All(log => log.CreatedById != currentUser.Id)
                                        : isAdmin && questionQueries.ReviewMode == ReviewMode.Reviewed
                                            ? question.IsReadyForAddingQuestionBank == true
                                            : isAdmin && questionQueries.ReviewMode == ReviewMode.UnderReview
                                                ? question.IsReadyForAddingQuestionBank == false
                                                : isQuestionSetter
                            )
                        ),
            question => question.Category,
            question => question.ReviewLogs,
            question => question.CreatedBy!
        );
    }


    public Question GetById(long id)
    {
        var currentUser = authenticationService.CurrentUserInWhichRole(
            out var isAdmin,
            out var isReviewer,
            out var isQuestionSetter
        );

        return repositoryWrapper.Question.GetFirstBy(
            question => question.Id == id && (
                isQuestionSetter
                    ? question.CreatedById == currentUser.Id
                    : isAdmin ||
                      (
                          isReviewer && question.Category.Reviewers.Any(reviewer =>
                              reviewer.AppUserId == currentUser.Id)
                      )
            ),
            question => question.Category,
            question => question.CreatedBy!
        );
    }

    public Question Create(QuestionDto question)
    {
        return repositoryWrapper.Question.Insert(mapper.Map<Question>(question));
    }

    public Question Update(long id, QuestionDto question)
    {
        question.Id = id;
        var currentUser = authenticationService.CurrentUserInWhichRole(
            out _,
            out _,
            out var isQuestionSetter
        );

        var prevQuestion = repositoryWrapper.Question.GetById(id);

        if (isQuestionSetter && prevQuestion.CreatedBy != currentUser) throw new UnauthorizedAccessException();
        if (prevQuestion.ReviewLogs.Count > 0) throw new DataException();

        return repositoryWrapper.Question.Update(mapper.Map<Question>(question));
    }

    public void Delete(long id)
    {
        var currentUser = authenticationService.CurrentUserInWhichRole(
            out _,
            out _,
            out var isQuestionSetter
        );
        var prevQuestion = repositoryWrapper.Question.GetById(id);

        if (isQuestionSetter && prevQuestion.CreatedBy != currentUser) throw new UnauthorizedAccessException();

        if (prevQuestion.IsAddedToQuestionBank == true) throw new DataException();

        repositoryWrapper.Question.Delete(id);
    }

    public static List<string> GetTypes()
    {
        return Enum.GetValues(typeof(QuestionType))
            .Cast<QuestionType>()
            .Select(v => v.ToString())
            .ToList();
    }

    public Question AddToQuestionBank(long id)
    {
        var question = repositoryWrapper.Question.GetById(id);
        question.IsAddedToQuestionBank = true;
        return repositoryWrapper.Question.Update(question);
    }
}