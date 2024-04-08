using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.Attribute;

public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value, ValidationContext validationContext)
    {
        return value is not IFormFile file
            ? new ValidationResult("Only file is allowed!")
            : (file.Length > maxFileSize
                ? new ValidationResult($"Maximum allowed file size is {maxFileSize} bytes.")
                : ValidationResult.Success)!;
    }
}