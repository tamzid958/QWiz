namespace QWiz.Helpers.Paginator;

public interface IUriService
{
    public Uri? GetPageUri(PaginationFilter filter, string route);
}