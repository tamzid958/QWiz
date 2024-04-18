using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.Paginator;

public class PaginationFilter
{
    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 20;
        Order = Order.Desc;
        Sort = "id";
    }

    public PaginationFilter(int pageNumber, int pageSize, string sort, Order order = Order.Desc)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 1 ? 1 : pageSize;
        Order = order;
        Sort = sort;
    }

    [FromQuery(Name = "page")] public int PageNumber { get; set; }

    [FromQuery(Name = "size")] public int PageSize { get; set; }

    [FromQuery(Name = "order")] public Order Order { get; set; }

    [FromQuery(Name = "sortBy")] public string Sort { get; set; }
}