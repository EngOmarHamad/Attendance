using Attendance.Utility.QueryParameters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Attendance.Web.ViewModels
{
    public class AllUsersViewModel
    {
        public UserQueryParameter QP { get; set; }
        public BasePageResult<UserViewModel> PageResult { get; set; }
        public IEnumerable<SelectListItem> lstPageSize { get; set; }
        public IEnumerable<SelectListItem> SortList { get; set; }
        public IEnumerable<SelectListItem> ListOfUsersNames { get; set; }
        public IEnumerable<SelectListItem> ListOfFamilyNames { get; set; }
        public IEnumerable<SelectListItem> ListOfEmails { get; set; }
        public IEnumerable<SelectListItem> ListOfAddress { get; set; }
    }
}
