using QWiz.Entities;
using QWiz.Helpers.HttpQueries;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ReviewerService(IRepositoryWrapper repositoryWrapper)
{
    public List<Reviewer> Get(ReviewerQueries reviewerQueries)
    {
        return repositoryWrapper.Reviewer.GetAll(
            o => o.CategoryId == reviewerQueries.CategoryId,
            "id",
            Order.Asc,
            reviewer => reviewer.AppUser
        );
    }
}