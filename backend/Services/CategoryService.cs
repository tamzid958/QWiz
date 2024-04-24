using System.Data;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class CategoryService(
    IRepositoryWrapper repositoryWrapper,
    AuthenticationService authenticationService
)
{
    public List<Category> Get()
    {
        var currentUser = authenticationService.CurrentUserInWhichRole(
            out _,
            out var isReviewer,
            out _
        );

        return repositoryWrapper.Category.GetAll(
            category => !isReviewer || category.Reviewers.Any(reviewer => reviewer.AppUserId == currentUser.Id),
            "id",
            Order.Asc,
            category => category.Reviewers.Select(x => x.AppUser),
            category => category.CreatedBy!
        );
    }

    public Category GetById(int id)
    {
        return repositoryWrapper.Category.GetFirstBy(o => o.Id == id,
            category => category.Reviewers.Select(x => x.AppUser)
        );
    }

    public Category Create(CategoryWithReviewer categoryWithReviewer)
    {
        var category = repositoryWrapper.Category.Insert(new Category
        {
            Name = categoryWithReviewer.Name
        });

        CreateMultiple(category, categoryWithReviewer.Reviewer.AppUserNames);

        return category;
    }

    public Category Update(int id, CategoryWithReviewer categoryWithReviewer)
    {
        var category = repositoryWrapper.Category.GetById(id);
        category.Name = categoryWithReviewer.Name;

        UpdateMultiple(category, categoryWithReviewer.Reviewer.AppUserNames);

        return repositoryWrapper.Category.Update(category);
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
        var category = repositoryWrapper.Category.GetById(id);
        if (category.Name == "Uncategorized") throw new DataException();

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
        repositoryWrapper.Reviewer.Delete(o => o.CategoryId == id);
    }

    private void CreateMultiple(Category category, List<string> appUserNames)
    {
        var approvers = appUserNames.ConvertAll(appUserName => new Reviewer
        {
            CategoryId = category.Id,
            AppUserId = repositoryWrapper.AppUser.GetFirstBy(o => o.UserName == appUserName).Id,
            CreatedById = category.CreatedBy!.Id
        });

        repositoryWrapper.Reviewer.Insert(approvers);
    }

    private void UpdateMultiple(Category category, List<string> appUserNames)
    {
        repositoryWrapper.Reviewer.Delete(o => o.Category == category);

        var approvers = appUserNames.ConvertAll(appUserName => new Reviewer
        {
            CategoryId = category.Id,
            AppUserId = repositoryWrapper.AppUser.GetFirstBy(o => o.UserName == appUserName).Id
        });

        repositoryWrapper.Reviewer.Insert(approvers);
    }
}