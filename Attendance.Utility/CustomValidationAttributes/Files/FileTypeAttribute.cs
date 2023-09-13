using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Utility.CustomValidationAttributes.Files
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly string[] _Filetype;

        public FileTypeAttribute(string[] filetype)
        {
            _Filetype = filetype;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (!_Filetype.Contains(Path.GetExtension(file.FileName).ToLower().Replace(".", "")))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
        public static string GetErrorMessage() => $" للاسف لا يمكنك رفع هذا النوع من الملفات الرجاء رفع صورة";

    }
}
