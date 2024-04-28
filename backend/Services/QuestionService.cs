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

        return isAdmin switch
        {
            true => GetForAdmin(request, paginationFilter, questionQueries),
            _ => isReviewer switch
            {
                true => GetForReviewer(request, paginationFilter, questionQueries, currentUser),
                _ => isQuestionSetter
                    ? GetForQuestionSetter(request, paginationFilter, questionQueries, currentUser)
                    : throw new DataException()
            }
        };
    }

    private PagedResponse<List<Question>> GetForQuestionSetter(
        HttpRequest request,
        PaginationFilter paginationFilter,
        QuestionQueries questionQueries,
        AppUser user
    )
    {
        return repositoryWrapper.Question.GetAll(
            paginationFilter,
            request,
            question =>
                (questionQueries.CategoryId == null || question.CategoryId == questionQueries.CategoryId) &&
                question.CreatedById == user.Id,
            question => question.ReviewLogs,
            question => question.Category,
            question => question.CreatedBy!
        );
    }

    private PagedResponse<List<Question>> GetForReviewer(
        HttpRequest request,
        PaginationFilter paginationFilter,
        QuestionQueries questionQueries, AppUser user
    )
    {
        return repositoryWrapper.Question.GetAll(
            paginationFilter,
            request,
            question =>
                (questionQueries.CategoryId == null || question.CategoryId == questionQueries.CategoryId) &&
                question.Category.Reviewers.Any(reviewer => reviewer.AppUserId == user.Id) &&
                (questionQueries.Reviewed == true
                    ? question.ReviewLogs.Any(log => log.CreatedById == user.Id)
                    : question.ReviewLogs.All(log => log.CreatedById != user.Id)),
            question => question.Category,
            question => question.CreatedBy!
        );
    }

    private PagedResponse<List<Question>> GetForAdmin(
        HttpRequest request,
        PaginationFilter paginationFilter,
        QuestionQueries questionQueries
    )
    {
        return repositoryWrapper.Question.GetAll(
            paginationFilter,
            request,
            question =>
                (questionQueries.CategoryId == null || question.CategoryId == questionQueries.CategoryId) &&
                (questionQueries.Reviewed == null ||
                 (question.IsReadyForAddingQuestionBank == true && questionQueries.Reviewed == true
                     ? question.IsAddedToQuestionBank != null : question.IsAddedToQuestionBank == null)) &&
                (questionQueries.IsAddedToQuestionBank == null ||
                 question.IsAddedToQuestionBank == questionQueries.IsAddedToQuestionBank),
            question => question.ReviewLogs,
            question => question.Category,
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
            out var isAdmin,
            out _,
            out var isQuestionSetter
        );

        var prevQuestion = repositoryWrapper.Question.GetFirstBy(
            ques => ques.Id == id,
            ques => ques.ReviewLogs
        );

        if (!isAdmin || prevQuestion.CreatedById != currentUser.Id)
            throw new UnauthorizedAccessException();
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

    public Question AddToQuestionBank(long id, bool accept)
    {
        var question = repositoryWrapper.Question.GetById(id);
        question.IsAddedToQuestionBank = accept;
        return repositoryWrapper.Question.Update(question);
    }
}