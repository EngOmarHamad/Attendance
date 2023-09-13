namespace Attendance.DataAccess.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        //public async Task SendNotificationAsync(NotificationModel notification)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", "Hello from signalR");

        //}
        public void SendToUser(string userId, string message)
        {
            Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
