using Attendance.Utility;

namespace Attendance.DataAccess.Services;
public class NotificationService : INotificationService
{
    private const string _baseURL = "";
    private readonly AttendanceDbContext _context;
    private readonly UserManager<UserModel> _userManager;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IAttendanceEmailService _attendanceEmailService;
    private readonly ILogger<NotificationService> _logger;
    public NotificationService(AttendanceDbContext context, UserManager<UserModel> userManager, IHubContext<NotificationHub> hubContext, ILogger<NotificationService> logger, IAttendanceEmailService attendanceEmailService)
    {
        _context = context;
        _userManager = userManager;
        _hubContext = hubContext;
        _logger = logger;
        _attendanceEmailService = attendanceEmailService;
    }
    public async Task NotifyUser(string text, string url, string userId, string type = "")
    {
        var appUser = await _userManager.FindByIdAsync(userId);

        NotificationModel notification = new()
        {
            Text = text,
            URL = url,
            UserId = userId,
            DateAdded = DateTime.Now,
            Seen = false,
            Type = type
        };
        _context.TblNotificationModel.Add(notification);
        _context.SaveChanges();
        var data = new
        {
            Title = notification.Text,
            notification.URL,
            notification.UserId,
            Date = notification.DateAdded.GetPrettyDate(),
            Image = appUser?.ProfileImage ?? "",
            notification.Color,
            notification.DateAdded,
            notification.DateAdded.Hour,
            notification.DateAdded.Minute,

        };
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
    }


    public async Task NotifyUserAsync(string userid, NotificationType type, params string?[] args)
    {
        var appUser = await _userManager.FindByIdAsync(userid);
        if (appUser == null)
        {
            _logger.LogError(message: $"NotifyUserAsync Err: {userid}  not exist");
            return;
        }
        NotificationsTemplate? notificationType = Constants.NotificationsData[type];
        //if (appUser.Email != null)
        //{
        //    await _attendanceEmailService.SendEmailAsync(email: appUser.Email, notificationType.Text, notificationType.Text);
        //}
        NotificationModel notification = new()
        {
            Text = notificationType.Text,
            Image = notificationType.IsMine ? notificationType.Image : appUser?.ProfileImage ?? "",
            URL = notificationType.URL,
            UserId = userid,
            DateAdded = DateTime.Now,
            Seen = false,
            Type = type.ToString(),
            Color = "success"
        };

        _context.TblNotificationModel.Add(notification);
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(message: $"NotifyUserAsync Err: ${ex.Message} + data: ${notification.Text}");
        }

        var user = _hubContext.Clients.User(userid);
        var notificationData = new
        {
            Title = notification.Text,
            notification.URL,
            notification.UserId,
            Date = notification.DateAdded.GetPrettyDate(),
            Image = notificationType.IsMine ? notificationType.Image : appUser?.ProfileImage ?? "",
            notificationType.Color,
            notification.DateAdded,
            notification.DateAdded.Hour,
            notification.DateAdded.Minute,
        };
        await user.SendAsync("ReceiveNotification", notificationData);
    }

    public async Task NotifyUserAsync(UserModel appUser, NotificationType type, params string?[] args)
    {

        NotificationsTemplate? notificationType = Constants.NotificationsData[type];
        if (appUser == null)
        {
            _logger.LogError(message: $"NotifyUserAsync Err: {appUser}  not exist");
            return;
        }
        NotificationModel notification = new()
        {
            Text = notificationType.Text,
            Image = notificationType.IsMine ? notificationType.Image : appUser?.ProfileImage ?? "",
            URL = notificationType.URL,
            UserId = appUser?.Id ?? "",
            DateAdded = DateTime.Now,
            DateSeen = DateTime.Now,
            Seen = false,
            Color = notificationType.Color,
            Type = type.ToString()
        };

        _context.TblNotificationModel.Add(notification);
        await _context.SaveChangesAsync();

        var user = _hubContext.Clients.User(appUser?.Id ?? "");
        var notificationData = new
        {
            Title = notification.Text,
            notification.URL,
            notification.UserId,
            Date = (notification.DateAdded.GetPrettyDate()),
            Image = notificationType.IsMine ? notificationType.Image : appUser?.ProfileImage ?? "",
            notificationType.Color,
            notification.DateAdded,
            notification.DateAdded.Hour,
            notification.DateAdded.Minute,
        };
        //if (appUser?.Email != null)
        //{
        //    await _attendanceEmailService.SendEmailAsync(email: appUser.Email, string.Format(notificationType.Text, args), string.Format(notificationType.Text, args));
        //}

        await user.SendAsync("ReceiveNotification", notificationData);
    }

    public async Task NotifyPermission(string permission, NotificationType type, params string?[] args)
    {
        try
        {
            var users = await _userManager.GetUsersForClaimAsync(new Claim("Permission", permission));
            var tasks = users.Select(user => NotifyUserAsync(user, type, args)).ToArray();
            await Task.WhenAll(tasks);

            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(message: $"NotifyUserAsync Err: ${ex.Message}");
        }
    }




}