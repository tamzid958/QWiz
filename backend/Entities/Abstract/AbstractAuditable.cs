using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QWiz.Entities.Abstract;

public class AbstractAuditable<TKey, TUser, TUserKey> : IAbstractAuditable<TKey, TUser, TUserKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; }

    public TUserKey? CreatedById { get; set; }
    [ForeignKey("CreatedById")] public TUser? CreatedBy { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }
}