namespace Attendance.Utility.QueryParameters
{
    public class ContractQueryParameter : BaseQueryParameter
    {
        public string? Name { get; set; }
        public int? ContractTypeId { get; set; }
        public ContractStatus? Status { get; set; }
    }
}
