using System.ComponentModel.DataAnnotations;
using QWiz.Entities.Enum;
using QWiz.Helpers.EntityMapper.DTOs.Abstract;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class UpdateAppUserDto : AbstractPersistenceDto<string>
{
    [Required] [Phone] public required string PhoneNumber { get; set; }
    [Required] public required string FullName { get; set; }

    [Required] [EmailAddress] public required string Email { get; set; }
    public required List<Role> Roles { get; set; }
}