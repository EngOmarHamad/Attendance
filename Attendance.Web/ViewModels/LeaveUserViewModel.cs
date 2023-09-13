namespace Attendance.Web.ViewModels;

public class AddLeaveUserViewModel
{
    public int LeaveTypeid { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime StartLeave { get; set; }

    [Required]
    public string? Reason { get; set; }

    public double? Numberofdays { get; set; }

    public bool Ismorning { get; set; }

}
