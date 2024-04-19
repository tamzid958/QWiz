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
        return repositoryWrapper.Category.GetFirstBy(o => o.Id == id,
            category => category.Approvers.Select(x => x.AppUser)
        );
    }

    public Category Create(CategoryWithApprover categoryWithApprover)
    {
        var category = repositoryWrapper.Category.Insert(new Category
        {
            Name = categoryWithApprover.Name
        });

        CreateMultiple(category, categoryWithApprover.Approver.AppUserNames);

        return category;
    }

    public Category Update(int id, CategoryWithApprover categoryWithApprover)
    {
        var category = repositoryWrapper.Category.GetById(id);
        category.Name = categoryWithApprover.Name;

        UpdateMultiple(category, categoryWithApprover.Approver.AppUserNames);

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

    private void CreateMultiple(Category category, List<string> appUserNames)
    {
        var approvers = appUserNames.ConvertAll(appUserName => new Approver
        {
            CategoryId = category.Id,
            AppUserId = repositoryWrapper.AppUser.GetFirstBy(o => o.UserName == appUserName).Id,
            CreatedById = category.CreatedBy!.Id
        });

        repositoryWrapper.Approver.Insert(approvers);
    }

    private void UpdateMultiple(Category category, List<string> appUserNames)
    {
        repositoryWrapper.Approver.Delete(o => o.Category == category);

        var approvers = appUserNames.ConvertAll(appUserName => new Approver
        {
            CategoryId = category.Id,
            AppUserId = repositoryWrapper.AppUser.GetFirstBy(o => o.UserName == appUserName).Id
        });

        repositoryWrapper.Approver.Insert(approvers);
    }
}