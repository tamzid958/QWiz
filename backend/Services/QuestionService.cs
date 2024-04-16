using AutoMapper;
using QWiz.Entities;
using QWiz.Entities.Enum;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class QuestionService(IRepositoryWrapper repositoryWrapper, CategoryService categoryService, IMapper mapper)
{
    public void MoveToUncategorized(int categoryId)
    {
        var defaultCategory = categoryService.CreateUnCategorized();

        var questions = repositoryWrapper.Question.GetAll(o => o.CategoryId == categoryId);

        questions.ForEach(o => o.CategoryId = defaultCategory.Id);

        repositoryWrapper.Question.Update(questions);
    }

    public static List<string> GetTypes()
    {
        return Enum.GetValues(typeof(QuestionType))
            .Cast<QuestionType>()
            .Select(v => v.ToString())
            .ToList();
    }

    public PagedResponse<List<Question>> Get(HttpRequest request, PaginationFilter paginationFilter)
    {
        return repositoryWrapper.Question.GetAll(paginationFilter, request);
    }

    public Question GetById(long id)
    {
        return repositoryWrapper.Question.GetById(id);
    }

    public Question Create(QuestionDto question)
    {
        return repositoryWrapper.Question.Insert(mapper.Map<Question>(question));
    }

    public Question Update(long id, QuestionDto question)
    {
        question.Id = id;
        return repositoryWrapper.Question.Update(mapper.Map<Question>(question));
    }

    public void Delete(long id)
    {
        repositoryWrapper.Question.Delete(id);
    }
}