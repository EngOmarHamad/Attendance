namespace Attendance.Models
{
    public class LeaveUserModel : BaseModel
    {
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserModel? User { get; set; }
        public int LeaveId { get; set; }

        [ForeignKey(nameof(LeaveId))]
        public virtual LeaveTypeModel? LeaveType { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartLeave { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndLeave { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        [MaxLength(1000)]
        public string? Reason { get; set; }
        [DataType(DataType.Html)]
        public double? CountDaysLeaveType { get; set; }
    }
}

