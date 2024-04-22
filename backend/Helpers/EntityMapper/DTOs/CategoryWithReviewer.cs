using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class CategoryWithReviewer
{
    [Required] [MaxLength(100)] public required string Name { get; set; }

    [Required] public required ReviewerDto Reviewer { get; set; }

    public class ReviewerDto
    {
        [MinLength(1)] public required List<string> AppUserNames { set; get; }
    }
}