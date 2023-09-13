namespace Attendance.Web.ViewModels;
public class AllStaffLeavesViewModel
{
    public StaffLeavesQueryParameter QP { get; set; }
    public BasePageResult<LeaveTypeUserViewModel> PageResult { get; set; }
    public IEnumerable<SelectListItem> lstPageSize { get; set; }
    public IEnumerable<SelectListItem> SortList { get; set; }
    public SelectList ListOfLeaveType { get; set; }
    public AddLeaveUserViewModel addLeaveUserViewModel { get; set; }
}
