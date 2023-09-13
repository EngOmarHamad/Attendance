namespace Attendance.Web.ViewModels
{
public class AllLeaveUserViewModel
{
    public UserLeaveQueryParameter? QP { get; set; }
    public BasePageResult<LeaveUserModel>? PageResult { get; set; }
    public IEnumerable<SelectListItem>? LstPageSize { get; set; }
    public IEnumerable<SelectListItem>? LstSortBy { get; set; }
    public IEnumerable<SelectListItem>? LstUser { get; set; }
    public SelectList ListOfLeaveType { get; set; }
    }
}
