using Attendance.DataAccess.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.IO;
using System.Security.Authentication;


namespace Attendance.Services
{
    public class AttendanceEmailService : IEmailSender, IAttendanceEmailService
    {

        private readonly EmailSettings? _emailSettings;
        private readonly IHostEnvironment _environment;

        public AttendanceEmailService(IOptions<EmailSettings> emailSetting, IHostEnvironment environment)
        {
            _emailSettings = emailSetting.Value;
            _environment = environment;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var backupDir = Path.Combine(_environment.ContentRootPath, "TempEmails");
                string floag = $"smtp{DateTime.Now:hhmmss}.log";
                MimeMessage emailMessage = new();
                MailboxAddress emailFrom = new(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new("Customer", email);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = subject;
                BodyBuilder emailBodyBuilder = new();
                emailBodyBuilder.HtmlBody = htmlMessage;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                await SaveToPickupDirectory(emailMessage, backupDir);

                SmtpClient emailClient = new(new MailKit.ProtocolLogger(floag));

                if (_emailSettings.UseSSL)
                {
                    emailClient.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await emailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
                }
                else
                {
                    await emailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.None);
                }

                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                await emailClient.SendAsync(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        private static async Task SaveToPickupDirectory(MimeMessage message, string pickupDirectory)
        {
            do
            {
                if (!Directory.Exists(pickupDirectory)) Directory.CreateDirectory(pickupDirectory);

                var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");////??
                Stream stream;
                try
                {
                    stream = File.Open(path, FileMode.CreateNew);
                }
                catch (IOException)
                {
                    if (File.Exists(path))
                        continue;
                    throw;
                }
                try
                {
                    using (stream)
                    {
                        using var filtered = new FilteredStream(stream);
                        filtered.Add(new SmtpDataFilter());
                        var options = FormatOptions.Default.Clone();
                        options.NewLineFormat = NewLineFormat.Dos;
                        await message.WriteToAsync(options, filtered);
                        await filtered.FlushAsync();
                        return;
                    }
                }
                catch
                {
                    File.Delete(path);
                    throw;
                }
            } while (true);
        }
    }
}
