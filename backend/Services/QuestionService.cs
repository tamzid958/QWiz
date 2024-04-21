using AutoMapper;
using QWiz.Entities;
using QWiz.Entities.Enum;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.HttpQueries;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class QuestionService(IRepositoryWrapper repositoryWrapper, IMapper mapper)

{
    public PagedResponse<List<Question>> Get(HttpRequest request, PaginationFilter paginationFilter,
        QuestionQueries questionQueries)
    {
        return repositoryWrapper.Question.GetAll(paginationFilter, request,
            question => questionQueries.CategoryId == null || question.CategoryId == questionQueries.CategoryId,
            question => question.Category,
            question => question.CreatedBy!
        );
    }

    public Question GetById(long id)
    {
        return repositoryWrapper.Question.GetFirstBy(
            question => question.Id == id,
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
        return repositoryWrapper.Question.Update(mapper.Map<Question>(question));
    }

    public void Delete(long id)
    {
        repositoryWrapper.Question.Delete(id);
    }

    public List<string> GetTypes()
    {
        return Enum.GetValues(typeof(QuestionType))
            .Cast<QuestionType>()
            .Select(v => v.ToString())
            .ToList();
    }
}