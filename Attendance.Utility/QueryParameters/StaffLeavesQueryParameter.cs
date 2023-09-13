namespace Attendance.Utility.QueryParameters
{
    public class StaffLeavesQueryParameter : BaseQueryParameter
    {
        public string? SearchTearm { get; set; }
        public string? StaffLeaveType { get; set; }
        public DateTime? StartLeaveTypeStart { get; set; }
        public DateTime? StartLeaveTypeEnd { get; set; }
        public DateTime? EndLeaveTypeStart { get; set; }
        public DateTime? EndLeaveTypeEnd { get; set; }
        public LeaveStatus? Status { get; set; }
        public string? UserId { get; set; }

    }
}