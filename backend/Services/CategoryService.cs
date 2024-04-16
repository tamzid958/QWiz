using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class CategoryService(
    IRepositoryWrapper repositoryWrapper,
    ApproverService approverService,
    QuestionService questionService)
{
    public List<Category> Get(HttpRequest request)
    {
        return repositoryWrapper.Category.GetAll();
    }

    public Category GetById(int id)
    {
        return repositoryWrapper.Category.GetById(id);
    }

    public Category Create(CategoryWithApprover categoryWithApprover)
    {
        var category = repositoryWrapper.Category.Insert(new Category
        {
            Name = categoryWithApprover.Name
        });

        approverService.CreateMultiple(category, categoryWithApprover.Approver.AppUserIds);

        return category;
    }

    public Category Update(int id, CategoryWithApprover categoryWithApprover)
    {
        var category = repositoryWrapper.Category.GetById(id);
        category.Name = categoryWithApprover.Name;

        approverService.UpdateMultiple(category, categoryWithApprover.Approver.AppUserIds);

        return category;
    }

    public Category CreateUnCategorized()
    {
        const string defaultCategory = "Uncategorized";

        var exists = repositoryWrapper.Category.Any(o => o.Name == defaultCategory);

        if (!exists)
            return repositoryWrapper.Category.Insert(new Category
            {
                Name = defaultCategory
            });

        return repositoryWrapper.Category.GetFirstBy(o => o.Name == defaultCategory);
    }

    public void Delete(int id)
    {
        questionService.MoveToUncategorized(id);
        approverService.DeleteByCategory(id);
        repositoryWrapper.Category.Delete(id);
    }
}