namespace Attendance.Models;
public class UserContractModel : BaseModel
{
    [Required]
    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserModel? User { get; set; }

    public int ContractTypeId { get; set; }

    [ForeignKey(nameof(ContractTypeId))]
    public virtual ContractTypeModel? ContractTypeModel { get; set; }


    [DataType(DataType.Date)]
    public DateTime ContractStartDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    public DateTime ContractEndDate { get; set; } = DateTime.Now;
}
