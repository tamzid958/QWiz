using System.ComponentModel.DataAnnotations;

namespace QWiz.Helpers.Attribute;

public class AllowedExtensionsAttribute(string[] extensions) : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return new ValidationResult("Only file is allowed!");
        var extension = Path.GetExtension(file.FileName);
        return !extensions.Contains(extension.ToLower())
            ? new ValidationResult($"Only {string.Join(", ", extensions)} extension are allowed!")
            : ValidationResult.Success!;
    }
}