namespace Attendance.Web.Dtos;

public class LeaveTypeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? MaxDuration { get; set; }
}
