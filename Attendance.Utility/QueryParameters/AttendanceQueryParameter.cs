
namespace Attendance.Utility.QueryParameters;

public class AttendanceQueryParameter : BaseQueryParameter
{
    public string? UserId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? Day { get; set; }
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateTo { get; set; }

    public AttendanceStatus? Status { get; set; }

}
