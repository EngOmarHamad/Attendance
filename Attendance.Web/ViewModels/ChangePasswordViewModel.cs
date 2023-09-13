namespace Attendance.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password), Required(ErrorMessage = "Pls,Enter the {0}"), Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Pls,Enter the {0}"), Display(Name = "New Password")]

        public string NewPassword { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Pls,Enter the {0}"), Display(Name = "Confirm New Password"), Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
