using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QWiz.Entities.Abstract;

namespace QWiz.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Category : AbstractAuditable<int, AppUser, string>
{
    [Required] [MaxLength(100)] public required string Name { get; set; }

    public ICollection<Reviewer> Reviewers { get; set; } = new HashSet<Reviewer>();

    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
}