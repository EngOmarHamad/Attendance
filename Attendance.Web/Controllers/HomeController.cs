using Microsoft.Extensions.Options;

namespace Attendance.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserModel> _userManager;
        private readonly IAttendanceServices _attendanceServices;
        private readonly AttendanceSettings _attendanceSettings;
        private readonly AttendanceDbContext _attendanceDbContext;

        public HomeController(IOptions<AttendanceSettings> options, ILogger<HomeController> logger, UserManager<UserModel> userManager, IAttendanceServices attendanceServices, AttendanceDbContext attendanceDbContext)
        {
            _attendanceSettings = options.Value;
            _logger = logger;
            _userManager = userManager;
            _attendanceServices = attendanceServices;
            _attendanceDbContext = attendanceDbContext;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {

            UserModel? user = await _userManager.GetUserAsync(User);
            var a = _attendanceSettings.AttendanceHour;
            var a1 = _attendanceSettings.DepartureHour;
            ViewBag.attendanceHour = _attendanceSettings.AttendanceHour;
            ViewBag.DepartureHour = _attendanceSettings.DepartureHour;
            if (user != null)
            {
                AttendanceModel? attendanceModel = user.ListOfAttendance?.SingleOrDefault(a => a.Day == DateTime.Now.Date);
                return View(attendanceModel);
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AttendanceModel attendanceModel)
        {

            string? userID = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userID)) { return NotFound(); }
            attendanceModel.SignInTime = DateTime.Now;
            attendanceModel.AttendenceStatus = _attendanceSettings.AttendanceHour.AddHours(1) >= DateTime.Now ? AttendanceStatus.Presences : AttendanceStatus.Late;
            attendanceModel.Day = DateTime.Today;
            attendanceModel.UserId = userID;
            await _attendanceServices.CreateAttendance(attendanceModel);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SignOut(AttendanceModel attendanceModel)
        {

            AttendanceModel attendance = await _attendanceServices.GetAttendancebyId(attendanceModel.Id);
            if (attendance == null) { return NotFound(); }
            attendance.SignOutTime = DateTime.Now;
            await _attendanceServices.EditAttendance(attendance);
            return RedirectToAction("Index");

        }
    }
}