namespace QWiz.Helpers.Paginator;

public static class PaginationHelper
{
    public static PagedResponse<List<T>> CreatePagedResponse<T>(this List<T> pagedData,
        PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
    {
        var response =
            new PagedResponse<List<T>>(pagedData,
                validFilter.PageNumber,
                validFilter.PageSize,
                validFilter.Order,
                validFilter.Sort);
        var totalPages = totalRecords / (double)validFilter.PageSize;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.NextPage =
            (validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(
                    new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize, validFilter.Sort),
                    route)
                : null)!;
        response.PreviousPage =
            (validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(
                    new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize, validFilter.Sort),
                    route)
                : null)!;
        response.FirstPage =
            uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize, validFilter.Sort), route)!;
        response.LastPage =
            uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize, validFilter.Sort),
                route)!;
        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;
        return response;
    }
}