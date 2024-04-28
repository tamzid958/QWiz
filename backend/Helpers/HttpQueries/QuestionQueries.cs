using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class QuestionQueries
{
    [FromQuery(Name = "categoryId")] public int? CategoryId { set; get; }

    [FromQuery(Name = "reviewed")] public bool? Reviewed { set; get; }

    [FromQuery(Name = "isAddedToQuestionBank")]
    public bool? IsAddedToQuestionBank { set; get; }
}