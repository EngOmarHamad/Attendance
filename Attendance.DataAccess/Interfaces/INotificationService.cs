namespace Attendance.DataAccess.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUser(string text, string url, string userid, string type = "");
        Task NotifyUserAsync(string userid, NotificationType type, params string?[] args);
        Task NotifyPermission(string permission, NotificationType type, params string?[] args);

    }
}
