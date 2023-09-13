namespace Attendance.Web.ViewModels;
public class AllContractViewModel
{
    public ContractQueryParameter QP { get; set; }
    public BasePageResult<UserContractModel> PageResult { get; set; }
    public IEnumerable<SelectListItem> lstPageSize { get; set; }
    public IEnumerable<SelectListItem> LstSortBy { get; set; }
    public IEnumerable<SelectListItem> lstContractType { get; set; }
    public IEnumerable<SelectListItem> lstUser { get; set; }
}
