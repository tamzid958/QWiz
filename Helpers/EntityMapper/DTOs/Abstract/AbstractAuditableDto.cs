using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.EntityMapper.DTOs.Abstract;

public class AbstractAuditableDto<TKey, TUserKey> : AbstractPersistenceDto<TKey>
{
    [Required] public TUserKey? OwnerId { set; get; }

    [Required] public DateTime CreatedAt { get; set; }

    [Required] public DateTime UpdatedAt { get; set; }
}