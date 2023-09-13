using System.Text.Json;

namespace Attendance.Web.Controllers
{
    public class SeedDataController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AttendanceDbContext _dbContext;

        public SeedDataController(IWebHostEnvironment webHostEnvironment, AttendanceDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }
        public async Task Index()
        {
            await SeedData2();
        }

        public async Task SeedData2()
        {
            List<AttendanceModel>? attendances = new();
            List<LeaveUserModel>? leaves = new();
            List<UserContractModel>? contracts = new();

            List<string> list = new() {
                "5e2b9206-9a81-49fb-aa25-310f67d1afba",
                "7d9aefd6-d281-4abb-8734-d6bd5f044abc",
                "b6133a44-892c-4d11-8eb6-39d7c6d433a1",
                "bee2a92c-1d1f-49c2-8881-e44f256a1e65" };

            List<int> listLeavesIdS = _dbContext.TblLeaveTypeModel.Select(x => x.Id).ToList();
            List<int> listContractIdS = _dbContext.TblContractType.Select(x => x.Id).ToList();

            for (int i = 0; i < 100; i++)
            {
                Random gen = new();
                var day = RandomDay();
                var levday = gen.Next(0, 16);
                contracts.Add(new UserContractModel
                {
                    ContractStartDate = day,
                    ContractEndDate = day.AddMonths(gen.Next(0, 7)),
                    ContractTypeId = listContractIdS[gen.Next(0, listContractIdS.Count)],
                    UserId = list[gen.Next(0, list.Count)],

                });
            }
            await _dbContext.TblUserContractModel.AddRangeAsync(contracts);
            await _dbContext.SaveChangesAsync();

            for (int i = 0; i < 100; i++)
            {
                Random gen = new();
                var day = RandomDay();
                var levday = gen.Next(0, 16);
                leaves.Add(new LeaveUserModel
                {
                    CountDaysLeaveType = levday,
                    Reason = RandomString(),
                    StartLeave = day,
                    Status = (LeaveStatus)gen.Next(0, 4),
                    EndLeave = day.AddDays(levday),
                    UserId = list[gen.Next(0, list.Count)],
                    LeaveId = listLeavesIdS[gen.Next(0, listLeavesIdS.Count)]
                });
            }
            await _dbContext.TblLeaveUserModel.AddRangeAsync(leaves);
            await _dbContext.SaveChangesAsync();

            for (int i = 0; i < 100; i++)
            {
                Random gen = new();
                var day = RandomDay();
                attendances.Add(new AttendanceModel
                {
                    Day = day,
                    SignInTime = day.AddHours(-4),
                    SignOutTime = day.AddHours(4),
                    AttendenceStatus = (AttendanceStatus)gen.Next(0, 4),
                    UserId = list[gen.Next(0, list.Count)]
                });
            }
            await _dbContext.TblAttendance.AddRangeAsync(attendances);
            await _dbContext.SaveChangesAsync();
        }
        private static DateTime RandomDay()
        {
            Random gen = new();
            DateTime start = new(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
        public static string RandomString()
        {
            Random gen = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 255).Select(s => s[gen.Next(s.Length)]).ToArray());
        }
        public async Task SeedData()
        {
            string _basePath = Path.Combine(_webHostEnvironment.WebRootPath, "data");
            using StreamReader reader = new(_basePath + "\\attendance.json");
            var text = reader.ReadToEnd();
            List<AttendanceModel>? attendances = JsonSerializer.Deserialize<List<AttendanceModel>>(text);
            if (attendances is not null)
            {
                await _dbContext.TblAttendance.AddRangeAsync(attendances);
                await _dbContext.SaveChangesAsync();
            }

            using StreamReader reader2 = new(_basePath + "\\contracts.json");
            var text2 = reader.ReadToEnd();
            List<UserContractModel>? contracts = JsonSerializer.Deserialize<List<UserContractModel>>(text2);
            if (contracts is not null)
            {
                await _dbContext.TblUserContractModel.AddRangeAsync(contracts);
                await _dbContext.SaveChangesAsync();
            }
            using StreamReader reader3 = new(_basePath + "\\leaves.json");
            var text3 = reader.ReadToEnd();
            List<LeaveUserModel>? leaves = JsonSerializer.Deserialize<List<LeaveUserModel>>(text3);
            if (leaves is not null)
            {
                await _dbContext.TblLeaveUserModel.AddRangeAsync(leaves);
                await _dbContext.SaveChangesAsync();
            }
            using StreamReader reader4 = new(_basePath + "\\notifications.json");
            var text4 = reader.ReadToEnd();
            List<NotificationModel>? notifications = JsonSerializer.Deserialize<List<NotificationModel>>(text4);
            if (notifications is not null)
            {
                await _dbContext.TblNotificationModel.AddRangeAsync(entities: notifications);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
