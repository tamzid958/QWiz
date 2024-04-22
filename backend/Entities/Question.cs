using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QWiz.Entities.Abstract;
using QWiz.Entities.Enum;

namespace QWiz.Entities;

public class Question : AbstractAuditable<long, AppUser, string>
{
    [MaxLength(400)] public required string Title { set; get; }

    [MaxLength(5000)] public string? Description { set; get; }

    [Required]
    [Column(TypeName = "varchar(30)")]
    public required QuestionType QuestionType { set; get; }

    public bool? IsAddedToQuestionBank { set; get; }

    public required int CategoryId { set; get; }

    [ForeignKey("CategoryId")] [Required] public required Category Category { set; get; }

    public ICollection<ApprovalLog> ApprovalLogs { get; set; } = new HashSet<ApprovalLog>();
}