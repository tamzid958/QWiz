using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class CategoryWithApprover
{
    [Required] [MaxLength(100)] public required string Name { get; set; }

    [Required] public required ApproverDto Approver { get; set; }

    public class ApproverDto
    {
        [MinLength(1)] public required List<string> AppUserNames { set; get; }
    }
}