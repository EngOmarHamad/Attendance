namespace Attendance.Web.ViewModels
{
    public class EditProfileSettingsViewModel : UserViewModel
    {
        public IFormFile? Image { get; set; }
        public new DateTime? DateOfBirth { get; set; }

    }
}
