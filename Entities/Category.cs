using System.ComponentModel.DataAnnotations;
using QWiz.Entities.Abstract;

namespace QWiz.Entities;

public class Category : AbstractAuditable<int, AppUser, string>
{
    [Required] [MaxLength(100)] public required string Name { get; set; }
    
    public ICollection<Approver> Approvers { get; set; } = new HashSet<Approver>();
    
    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
}