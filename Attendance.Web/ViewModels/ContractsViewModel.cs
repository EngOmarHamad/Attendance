namespace Attendance.Web.ViewModels
{
    public class ContractsViewModel : BaseModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Contract { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public string Status { get; set; }
    }
}
