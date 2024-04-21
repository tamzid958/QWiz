using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class QuestionQueries
{
    [FromQuery(Name = "categoryId")] public int? CategoryId { set; get; }
}