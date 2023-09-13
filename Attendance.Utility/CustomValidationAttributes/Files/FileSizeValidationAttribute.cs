using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Utility.CustomValidationAttributes.Files
{
    public class FileSizeValidationAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public FileSizeValidationAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return file.Length <= _maxFileSize ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
        }
        public static string GetErrorMessage() => $"الرجاء ادخال صورة اقل من 10 ميجا";
    }
}
