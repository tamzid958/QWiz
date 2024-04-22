using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class ApprovalLogQueries
{
    [FromQuery(Name = "questionId")] public long? QuestionId { set; get; }
}