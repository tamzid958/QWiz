using QWiz.Entities;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ApproverService(IRepositoryWrapper repositoryWrapper)
{
    public List<Approver> CreateMultiple(Category category, List<string> appUserIds)
    {
        var approvers = appUserIds.ConvertAll(appUserId => new Approver
        {
            CategoryId = category.Id,
            AppUserId = appUserId
        });

        return repositoryWrapper.Approver.Insert(approvers);
    }

    public List<Approver> UpdateMultiple(Category category, List<string> appUserIds)
    {
        repositoryWrapper.Approver.Delete(o => o.Category == category);

        var approvers = appUserIds.ConvertAll(appUserId => new Approver
        {
            CategoryId = category.Id,
            AppUserId = appUserId
        });

        return repositoryWrapper.Approver.Insert(approvers);
    }

    public void DeleteByCategory(int id)
    {
        repositoryWrapper.Approver.Delete(o => o.CategoryId == id);
    }
}