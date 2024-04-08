using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QWiz.Entities.Abstract;

namespace QWiz.Entities;

public class Approver: AbstractAuditable<int, AppUser, string>
{
    
    public required int CategoryId { set; get; }
    
    [ForeignKey("CategoryId")] [Required] public required Category Category { set; get; }
    
    [Length(maximumLength:36, minimumLength: 36)] public required string AppUserId { set; get; }
    
    [ForeignKey("AppUserId")] [Required] public required AppUser AppUser { set; get; }
}