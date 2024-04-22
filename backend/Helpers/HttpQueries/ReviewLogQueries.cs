using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class ReviewLogQueries
{
    [FromQuery(Name = "questionId")] public long? QuestionId { set; get; }
}