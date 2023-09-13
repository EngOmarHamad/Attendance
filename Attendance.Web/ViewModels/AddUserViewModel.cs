using Attendance.Utility.CustomValidationAttributes.DateTimes;

namespace Attendance.Web.ViewModels;
public class AddUserViewModel : UserViewModel
{
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
    public int? Claim { get; set; }
    [Required]
    public IFormFile? Image { get; set; }


}
