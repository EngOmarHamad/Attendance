namespace Attendance.Web.Controllers;

[Authorize("Staff")]
public class StaffAttendancesController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IAttendanceServices _attendanceServices;

    public StaffAttendancesController(UserManager<UserModel> userManager, IAttendanceServices attendanceServices)
    {
        _userManager = userManager;
        _attendanceServices = attendanceServices;
    }
    public IActionResult Index()
    {
        return View(new AllAttendanceViewModel()
        {
            LstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
            LstSortBy = Constants.AttendanceSortBy,
        });
    }
    public async Task<IActionResult> GetAttendancesAsync(AttendanceQueryParameter QP)
    {
        IQueryable<AttendanceViewModel> list = (_attendanceServices.GetFilteredDataAttendances(QP))
            .Select(a =>
            new AttendanceViewModel()
            {
                Day = a.Day.GetSpecialDateFromat(),
                DayOfWeek = a.Day.DayOfWeek.ToString(),
                SignInTime = a.SignInTime?.GetSpecialTimeFromat(),
                SignOutTime = a.SignOutTime?.GetSpecialTimeFromat(),
                Status = a.AttendenceStatus.ToString(),
                UserId = a.UserId,
                UserName = a.User?.UserName
            }).AsQueryable();
        return Ok(list.GetPageResult(QP));
    }
}
