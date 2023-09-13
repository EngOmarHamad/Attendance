namespace Attendance.Models
{
    public class UserModel : IdentityUser
    {
        public string? ProfileImage { get; set; }
        [RegularExpression("^[A-Za-z\\d_-]+$")]
        public string? FirstName { get; set; }
        [RegularExpression("^[A-Za-z\\d_-]+$")]
        public string? FamilyName { get; set; }
        [RegularExpression(@"^[\u0600-\u06FF\d_-]+$")]
        public string? FirstNameAr { get; set; }
        [RegularExpression(@"^[\u0600-\u06FF\d_-]+$")]
        public string? FamilyNameAr { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? AboutMe { get; set; } = "";
        public string? Major { get; set; }
        public string? Status { get; set; } = UserStatus.Active.ToString();

        [DataType(DataType.Date), DateValidate]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.Date), DateValidate]
        public DateTime? JoinedDate { get; set; } = DateTime.Now;

        public virtual ICollection<AttendanceModel>? ListOfAttendance { get; set; }
        public virtual ICollection<LeaveUserModel>? ListOfLeaveUser { get; set; }
        public virtual ICollection<UserContractModel>? ListOfContracts { get; set; }
    }
}
