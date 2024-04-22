using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class ApproverQueries
{
    [FromQuery(Name = "categoryId")] public required int CategoryId { set; get; }
}