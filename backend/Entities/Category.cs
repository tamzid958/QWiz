using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QWiz.Entities.Abstract;

namespace QWiz.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Category : AbstractPersistence<int>
{
    [Required] [MaxLength(100)] public required string Name { get; set; }

    public ICollection<Approver> Approvers { get; set; } = new HashSet<Approver>();

    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
}