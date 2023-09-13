namespace Attendance.Web.ViewModels;
public class AllAttendanceViewModel
{
    public AttendanceQueryParameter? QP { get; set; }
    public BasePageResult<AttendanceModel>? PageResult { get; set; }
    public IEnumerable<SelectListItem>? LstPageSize { get; set; }
    public IEnumerable<SelectListItem>? LstSortBy { get; set; }
    public IEnumerable<SelectListItem>? LstUser { get; set; }

}

