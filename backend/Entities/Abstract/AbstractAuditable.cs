using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QWiz.Entities.Abstract;

public class AbstractAuditable<TKey, TUser, TUserKey> : AbstractPersistence<TKey>
{
    public TUserKey? CreatedById { set; get; }

    [ForeignKey("CreatedById")] public TUser? CreatedBy { set; get; }

    [Required] public DateTime CreatedAt { get; set; }

    [Required] public DateTime UpdatedAt { get; set; }
}