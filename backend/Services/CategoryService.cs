using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class CategoryService(
    IRepositoryWrapper repositoryWrapper)
{
    public List<Category> Get(HttpRequest request)
    {
        return repositoryWrapper.Category.GetAll(
            category => category.Approvers.Select(x => x.AppUser),
            category => category.CreatedBy!
        );
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

        CreateMultiple(category, categoryWithApprover.Approver.AppUserIds);

        return category;
    }

    public Category Update(int id, CategoryWithApprover categoryWithApprover)
    {
        var category = repositoryWrapper.Category.GetById(id);
        category.Name = categoryWithApprover.Name;

        UpdateMultiple(category, categoryWithApprover.Approver.AppUserIds);

        return category;
    }

    private Category CreateUnCategorized()
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
        MoveToUncategorized(id);
        DeleteByCategory(id);
        repositoryWrapper.Category.Delete(id);
    }

    private void MoveToUncategorized(int categoryId)
    {
        var defaultCategory = CreateUnCategorized();

        var questions = repositoryWrapper.Question.GetAll(o => o.CategoryId == categoryId);

        questions.ForEach(o => o.CategoryId = defaultCategory.Id);

        repositoryWrapper.Question.Update(questions);
    }


    private void DeleteByCategory(int id)
    {
        repositoryWrapper.Approver.Delete(o => o.CategoryId == id);
    }

    private void CreateMultiple(Category category, List<string> appUserIds)
    {
        var approvers = appUserIds.ConvertAll(appUserId => new Approver
        {
            CategoryId = category.Id,
            AppUserId = appUserId,
            CreatedById = category.CreatedBy!.Id
        });

        repositoryWrapper.Approver.Insert(approvers);
    }

    private void UpdateMultiple(Category category, List<string> appUserIds)
    {
        repositoryWrapper.Approver.Delete(o => o.Category == category);

        var approvers = appUserIds.ConvertAll(appUserId => new Approver
        {
            CategoryId = category.Id,
            AppUserId = appUserId
        });

        repositoryWrapper.Approver.Insert(approvers);
    }
}