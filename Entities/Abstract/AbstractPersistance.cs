using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QWiz.Entities.Abstract;

public abstract class AbstractPersistence<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; } = default!;

    [DefaultValue(true)] public bool IsActive { get; set; }
}