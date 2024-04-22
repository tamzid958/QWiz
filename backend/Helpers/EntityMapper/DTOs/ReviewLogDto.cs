using System.ComponentModel.DataAnnotations;
using QWiz.Entities.Abstract;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class ReviewLogDto : AbstractPersistence<long>
{
    public required long QuestionId { set; get; }
    public bool IsApproved { set; get; }
    [MaxLength(600)] public string? Comment { set; get; }
}