namespace Attendance.DataAccess.Interfaces
{
    public interface IAttendanceEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
