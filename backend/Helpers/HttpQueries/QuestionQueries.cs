using Microsoft.AspNetCore.Mvc;

namespace QWiz.Helpers.HttpQueries;

public class QuestionQueries
{
    [FromQuery(Name = "categoryId")] public int? CategoryId { set; get; }

    [FromQuery(Name = "isReadyForAddingQuestionBank")]
    public bool? IsReadyForAddingQuestionBank { set; get; }

    [FromQuery(Name = "issAddedToQuestionBank")]
    public bool? IsAddedToQuestionBank { set; get; }
}