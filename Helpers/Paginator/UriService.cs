using Microsoft.AspNetCore.WebUtilities;

namespace QWiz.Helpers.Paginator;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri? GetPageUri(PaginationFilter filter, string route)
    {
        var endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri =
            QueryHelpers.AddQueryString(endpointUri.ToString(), "page", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "size", filter.PageSize.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "sortBy", filter.Sort!.ToLower());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "order", filter.Order.ToString().ToLower());
        return new Uri(modifiedUri);
    }
}