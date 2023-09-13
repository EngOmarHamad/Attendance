namespace Attendance.Web.ViewModels;

public class LeaveTypeUserViewModel
{
    public int LeaveUserId { get; set; }

    public string? UserName { get; set; }
    public string? LeaveTypeName { get; set; }

    public string StartLeaveType { get; set; }
    public string EndLeaveType { get; set; }
    public string? Reason { get; set; }
    public LeaveStatus? Status { get; set; }
}
