namespace Attendance.Models
{
    public class LeaveTypeModel : BaseModel
    {
        [MaxLength(255)]
        public string? Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public int? MaxDuration { get; set; }
    }
}
