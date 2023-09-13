using Dapper;
using Microsoft.Extensions.Options;

namespace Attendance.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IAttendanceServices _attendanceServices;
        private readonly ILeaveUserService _leaveUserService;
        private readonly AttendanceSettings _attendanceSettings;
        private readonly IConfiguration _configuration;


        public HomeController(UserManager<UserModel> userManager, IAttendanceServices attendanceServices, ILeaveUserService leaveUserService, IOptions<AttendanceSettings> attendanceSettings, IConfiguration configuration)
        {
            _userManager = userManager;
            _attendanceServices = attendanceServices;
            _leaveUserService = leaveUserService;
            _attendanceSettings = attendanceSettings.Value;
            _configuration = configuration;
        }


        [Route("/{area}/")]
        public async Task<IActionResult> Index()
        {
            DateTime dateTime = _attendanceSettings.DepartureHour;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            RecurringJob.AddOrUpdate(() => Send(), Cron.Daily(hour, minute));

            var d = DateTime.Now.Date.AddDays(-7);
            List<string> strings = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                strings.Add(d.AddDays(i).ToString("dddd"));
            }
            try
            {
                using System.Data.SqlClient.SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));

                //var cmd= "SELECT TodayAttendenceCount,LeaveTypeCount,ActiveUserContracts,StaffMembersCount,ContractTypeCount FROM ITSCAttendanceCounters";

                //if (connection.State == System.Data.ConnectionState.Closed)
                //{
                //    await connection.OpenAsync();
                //}
                //using SqlCommand sqlCommand = new(cmd, connection); ;
                //SqlDataReader reader = sqlCommand.ExecuteReader();
                //while (await reader.ReadAsync())
                //{

                //    reader.GetInt32(reader.GetOrdinal(""));
                //}
                //if (connection.State == System.Data.ConnectionState.Open)
                //{
                //    await connection.CloseAsync();
                //}

                ITSCAttendanceCountersViewModel? counters = (await connection.QueryAsync<ITSCAttendanceCountersViewModel>("SELECT TodayAttendenceCount,LeaveTypeCount,ActiveUserContracts,StaffMembersCount,ContractTypeCount FROM ITSCAttendanceCounters", commandType: System.Data.CommandType.Text)).SingleOrDefault();

                return View(new HomeViewModel() { counters = counters });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


            // List<AttendanceForLastWeekViewModel> attendanceForLasts7Days = new();

            //for (int i = 0; i < strings.Count; i++)
            //{
            //    if (i < attendanceForLasts.Count - 1)
            //    {
            //        if (strings[i].Equals(attendanceForLasts[i].DayName, StringComparison.OrdinalIgnoreCase))
            //            attendanceForLasts7Days.Add(attendanceForLasts[i]);
            //    }
            //    else
            //        attendanceForLasts7Days.Add(new AttendanceForLastWeekViewModel { DayName = strings[i], AttendanceCount = 0 });

            //}
        }

        [HttpGet("/GetAttendances")]
        public async Task<IActionResult> GetAttendancesAsync()
        {
            List<AttendanceViewModel> attendances = (await _attendanceServices.GetAllAttendance()).Where(at => at.Day.Date == DateTime.Now.Date && at.AttendenceStatus == (int)AttendanceStatus.Presences).Select(a => new AttendanceViewModel()
            {
                Day = a.Day.GetSpecialDateFromat(),
                DayOfWeek = a.Day.DayOfWeek.ToString(),
                SignInTime = a.SignInTime?.GetSpecialTimeFromat(),
                SignOutTime = a.SignOutTime?.GetSpecialTimeFromat(),
                Status = a.AttendenceStatus.ToString(),
                UserId = a.UserId,
                UserName = a.User?.UserName
            }).ToList();

            return Ok(attendances);
        }
        [HttpGet("/GetAttendances2")]
        public async Task<IActionResult> GetAttendances2Async()
        {
            List<AttendanceViewModel> attendances = (await _attendanceServices.GetAllAttendance()).Where(at => at.Day.Date == DateTime.Now.Date && at.AttendenceStatus != (int)AttendanceStatus.Presences).Select(a => new AttendanceViewModel()
            {
                Day = a.Day.GetSpecialDateFromat(),
                DayOfWeek = a.Day.DayOfWeek.ToString(),
                SignInTime = a.SignInTime?.GetSpecialTimeFromat(),
                SignOutTime = a.SignOutTime?.GetSpecialTimeFromat(),
                Status = a.AttendenceStatus.ToString(),
                UserId = a.UserId,
                UserName = a.User?.UserName
            }).ToList();

            return Ok(attendances);
        }
        public async Task Send()
        {
            List<UserModel> users = await _userManager.Users.Where(u => u.Status == UserStatus.Active.ToString()).ToListAsync();
            List<AttendanceModel> AllAttendance = await _attendanceServices.GetAllAttendance();

            AllAttendance = AllAttendance.Where(x => x.Day == DateTime.Now.Date).ToList();
            List<LeaveUserModel> Leaves = await _leaveUserService.GetAllLeaveUsers();

            Leaves = Leaves.Where(x => x.StartLeave <= DateTime.Now.Date && x.EndLeave >= DateTime.Now.Date).ToList();


            for (int i = 0; i < users.Count; i++)
            {
                AttendanceModel? attendance = AllAttendance.SingleOrDefault(x => x.UserId == users[i].Id);
                LeaveUserModel? leave = Leaves.SingleOrDefault(x => x.UserId == users[i].Id);

                if (leave == null)
                {



                    if (attendance != null)
                    {

                        if (attendance.SignOutTime == null && attendance.SignInTime != null)
                        {
                            attendance.SignOutTime = DateTime.Now;
                            await _attendanceServices.EditAttendance(attendance);
                        }
                    }
                    else
                    {

                        AttendanceModel model = new()
                        {
                            Day = DateTime.Today,
                            UserId = users[i].Id,
                            AttendenceStatus = AttendanceStatus.Absent
                        };
                        await _attendanceServices.CreateAttendance(model);

                    }

                }
                else
                {

                    AttendanceModel model = new()
                    {
                        Day = DateTime.Today,
                        UserId = users[i].Id,
                        AttendenceStatus = AttendanceStatus.Leave
                    };
                    await _attendanceServices.CreateAttendance(model);

                }


            }
            Console.WriteLine($"taaaaaaaaaaaaaaaariq{DateTime.Now}");

        }
    }
}