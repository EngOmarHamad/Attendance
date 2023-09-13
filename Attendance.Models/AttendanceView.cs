namespace Attendance.Models;
public class AttendanceView
{
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public DateTime? Day { get; set; }
    public DateTime? SignInTime { get; set; }
    public DateTime? SignOutTime { get; set; }
    public int AttendenceStatus { get; set; }
}
