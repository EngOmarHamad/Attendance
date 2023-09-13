using System.Data;
using System.Linq.Dynamic.Core;

namespace Attendance.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AttendanceController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IAttendanceServices _attendanceServices;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AttendanceController(UserManager<UserModel> userManager, IAttendanceServices attendanceServices, IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _attendanceServices = attendanceServices;
        _webHostEnvironment = webHostEnvironment;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    }
    public async Task<IActionResult> IndexAsync()
    {
        List<UserModel> users = await _userManager.Users.ToListAsync();

        SelectList lstuser = new(users, nameof(UserModel.Id), nameof(UserModel.UserName));
        return View(new AllAttendanceViewModel()
        {
            LstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
            LstSortBy = Constants.AttendanceSortBy,
            LstUser = lstuser
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
