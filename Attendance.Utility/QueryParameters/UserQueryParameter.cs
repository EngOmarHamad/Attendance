namespace Attendance.Utility.QueryParameters
{
    public class UserQueryParameter : BaseQueryParameter
    {
        public string? FirstName { get; set; }
        public string? FirstNameAr { get; set; }
        public string? FamilyName { get; set; }
        public string? FamilyNameAr { get; set; }
        public int? Gender { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? SearchTearm { get; set; }
        public int? Status { get; set; }
        public bool? OnlyStaff { get; set; }
    }
}
