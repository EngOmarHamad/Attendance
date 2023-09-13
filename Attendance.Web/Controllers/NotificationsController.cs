namespace Web.Controllers
{
    [Authorize(Policy = "All")]
    public class NotificationsController : Controller
    {
        private readonly AttendanceDbContext _context;
        public NotificationsController(AttendanceDbContext context) { _context = context; }
        //public async Task<ActionResult> IndexAsync()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var data = await _context.TblNotificationModel.Where(x => x.UserId == userId)
        //        .OrderByDescending(x => x.DateAdded).Take(10).ToListAsync();
        //    return View(data);
        //}
        [HttpGet]
        public async Task<IActionResult> GetPartialAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return NotFound();
            }
            var lst = await _context.TblNotificationModel.Where(x => x.UserId == userId && x.Seen != true)
                .OrderByDescending(x => x.DateAdded).Take(5).ToListAsync();
            // ViewBag.NotificationCount = _context.TblNotificationModel.Count(x => x.UserId == userId && x.Seen != true);
            return PartialView("_UserNotification", lst);
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationsAsync(int page, int size)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return NotFound();
            }
            var lst = await _context.TblNotificationModel.Where(x => x.UserId == userId).OrderByDescending(x => x.DateAdded).Skip(page * size).Take(size).ToListAsync();

            return Ok(lst);
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationCountAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return NotFound();
            }
            return Json(await _context.TblNotificationModel.CountAsync(x => x.UserId == userId && x.Seen != true));
        }
        [HttpPost]
        public async Task<JsonResult> ResetNotificationCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lst = _context.TblNotificationModel.Where(x => x.UserId == userId && x.Seen != true);
            await lst.ForEachAsync(x => x.Seen = true);

            await _context.SaveChangesAsync();
            return new JsonResult(new { Status = 1 });
        }
        [HttpPost]
        public async Task<ActionResult> ReadNotification(int id)
        {
            NotificationModel? notification = _context.TblNotificationModel.FirstOrDefault(x => x.Id == id && x.Seen != true);

            if (notification is null)
            {
                return NotFound();
            }
            notification.Seen = true;
            await _context.SaveChangesAsync();
            return new JsonResult(new { Status = 1, Data = notification });
        }


    }
}
