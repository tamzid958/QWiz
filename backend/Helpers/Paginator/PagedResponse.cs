using Newtonsoft.Json;

namespace QWiz.Helpers.Paginator;

public class PagedResponse<T> : Response<T>
{
    public PagedResponse(T data, int pageNumber, int pageSize, Order order, string sort)
    {
        Data = data;
        Message = null;
        Succeeded = true;
        Errors = null;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Order = order;
        Sort = sort;
    }

    [JsonProperty("sortBy")] public string Sort { get; set; }

    [JsonProperty("page")] public int PageNumber { get; set; }

    [JsonProperty("size")] public int PageSize { get; set; }

    public Uri FirstPage { get; set; } = null!;

    public Uri LastPage { get; set; } = null!;

    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }

    public Uri NextPage { get; set; } = null!;

    public Uri PreviousPage { get; set; } = null!;

    [JsonProperty("order")] public Order Order { get; set; }
}