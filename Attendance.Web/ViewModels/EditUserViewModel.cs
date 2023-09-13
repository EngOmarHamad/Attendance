namespace Attendance.Web.ViewModels;

public class EditUserViewModel : UserViewModel
{
    //[Required, DataType(DataType.Password)]
    //public string? Password { get; set; }
    public int? Claim { get; set; }
    public IFormFile? Image { get; set; }
    public new DateTime? DateOfBirth { get; set; }

}
