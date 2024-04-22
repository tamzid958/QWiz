using QWiz.Entities;
using QWiz.Helpers.HttpQueries;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ApproverService(IRepositoryWrapper repositoryWrapper)
{
    public List<Approver> Get(ApproverQueries approverQueries)
    {
        return repositoryWrapper.Approver.GetAll(
            o => o.CategoryId == approverQueries.CategoryId
        );
    }
}