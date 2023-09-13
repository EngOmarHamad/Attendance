namespace Attendance.Web.ViewModels
{
    public class AttendanceViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DayOfWeek { get; set; }
        public string? Day { get; set; }
        public string? SignInTime { get; set; }
        public string? SignOutTime { get; set; }
        public string Status { get; set; }
    }
}
