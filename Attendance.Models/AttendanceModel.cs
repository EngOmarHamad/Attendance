
namespace Attendance.Models
{
    public class AttendanceModel : BaseModel
    {

        [DataType(DataType.Date)]
        public DateTime Day { get; set; }

        [DataType(DataType.Time)]
        public DateTime? SignInTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? SignOutTime { get; set; }

        public AttendanceStatus AttendenceStatus { get; set; } = AttendanceStatus.Presences;
        public string? UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual UserModel? User { get; set; }
    }
}
