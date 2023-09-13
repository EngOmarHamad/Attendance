using Attendance.Utility.CustomValidationAttributes.DateTimes;

namespace Attendance.Web.ViewModels;
public class UserViewModel : BaseViewModel<string>
{
    [RegularExpression("^05(9|6)[0-9]{3}[0-9]{4}$", ErrorMessage = "Enter the correct phone Number")]
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? ProfileImage { get; set; }
    public string? FullName { get => string.Concat(FirstName, " ", FamilyName); }
    [RegularExpression("^[\\sA-Za-z\\d_-]+$", ErrorMessage = "Please Enter the First Name in English")]
    public string? FirstName { get; set; }
    [RegularExpression("^[\\sA-Za-z\\d_-]+$", ErrorMessage = "Please Enter the Family Name in English")]
    public string? FamilyName { get; set; }
    [RegularExpression(@"^[\s\u0600-\u06FF\d_-]+$", ErrorMessage = "Please Enter the First Name in Arabic")]
    public string? FirstNameAr { get; set; }
    [RegularExpression(@"^[\s\u0600-\u06FF\d_-]+$", ErrorMessage = "Please Enter the Family Name in Arabic")]
    public string? FamilyNameAr { get; set; }
    public string? Address { get; set; }
    public int? Gender { get; set; }
    public string? Major { get; set; }
    public string? AboutMe { get; set; }
    public string? CurrentContractName { get; set; }
    public bool? HasContract { get; set; }
    public string? Status { get; set; } = UserStatus.Active.ToString();

    public string? DateOfBirth { get; set; }

    [Required, DateValidateAttribute]
    public DateTime Databirth { get; set; }
    public string? JoinedDate { get; set; }
}