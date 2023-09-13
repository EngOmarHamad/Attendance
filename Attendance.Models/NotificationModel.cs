namespace Attendance.Models
{
    public class NotificationModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Text { get; set; }
        // public string Body { get; set; }
        public string URL { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateSeen { get; set; }
        public bool Seen { get; set; }
        public bool IsMine { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public virtual UserModel ApplicationUser { get; set; }
    }
}
