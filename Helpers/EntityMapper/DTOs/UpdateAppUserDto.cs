using System.ComponentModel.DataAnnotations;
using QWiz.Helpers.EntityMapper.DTOs.Abstract;

namespace QWiz.Helpers.EntityMapper.DTOs;

public class UpdateAppUserDto : AbstractPersistenceDto<string>
{
    [Required] [Phone] public string PhoneNumber { get; set; } = null!;

    [Required] public string FullName { get; set; } = null!;
}