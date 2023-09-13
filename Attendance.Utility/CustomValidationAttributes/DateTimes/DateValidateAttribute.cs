using System.ComponentModel.DataAnnotations;

namespace Attendance.Utility.CustomValidationAttributes.DateTimes;
public class DateValidateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var date = (DateTime)value;

        var cuurentdate = DateTime.Now;

        if (date > cuurentdate)
        {
            return new ValidationResult(GetErrorMessage());
        }
        return ValidationResult.Success;
    }
    public static string GetErrorMessage() => "The date of birth must be less than the current date";
}