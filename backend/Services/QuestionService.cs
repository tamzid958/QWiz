using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class QuestionService(IRepositoryWrapper repositoryWrapper, CategoryService categoryService)
{
    public void MoveToUncategorized(int categoryId)
    {
        var defaultCategory = categoryService.CreateUnCategorized();

        var questions = repositoryWrapper.Question.GetAll(o => o.CategoryId == categoryId);

        questions.ForEach(o => o.CategoryId = defaultCategory.Id);

        repositoryWrapper.Question.Update(questions);
    }
}