using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QWiz.Entities.Enum;
using QWiz.Helpers.EntityMapper.DTOs.Abstract;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class QuestionDto : AbstractPersistenceDto<long>
{
    [MaxLength(400)] public required string Title { set; get; }

    [MaxLength(5000)] public string? Description { set; get; }

    [Required]
    [Column(TypeName = "varchar(30)")]
    public required QuestionType QuestionType { set; get; }

    public required int CategoryId { set; get; }
}