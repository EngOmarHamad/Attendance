using Microsoft.AspNetCore.Mvc.Rendering;

namespace Attendance.Utility;

public static class Constants
{
    public static List<string> PageSizeList => new() { "5", "10", "15", "20", "25" };
    public static IEnumerable<SelectListItem> AttendanceSortBy => new List<SelectListItem>
    {
         new SelectListItem() { Text = "Day Of Week", Value ="DayOfWeek" },
         new SelectListItem() { Text = "Day", Value ="Day" },
         new SelectListItem() { Text = "Sign In Time", Value ="SignInTime" },
         new SelectListItem() { Text = "Sign Out Time", Value ="SignOutTime" },
    };

    public static IEnumerable<SelectListItem> LeavesUserSortBy => new List<SelectListItem>
    {
         new SelectListItem() { Text = "Status", Value ="Status" },
         new SelectListItem() { Text = "LeaveTypeName", Value ="LeaveTypeName" },
         new SelectListItem() { Text = "StartLeaveType", Value ="StartLeaveType" },
         new SelectListItem() { Text = "EndLeaveType", Value ="EndLeaveType" },
    };

    public static IEnumerable<SelectListItem> UserSortBy => new List<SelectListItem>
    {
            new SelectListItem() { Text = "First Name", Value = "FirstName" },
            new SelectListItem() { Text = "First Name Ar", Value = "FirstNameAr" },
            new SelectListItem() { Text = "Family Name", Value = "FamilyName" },
            new SelectListItem() { Text = "Family Name Ar", Value = "FamilyNameAr" },
            new SelectListItem() { Text = "Date Of Birth", Value =  "DateOfBirth" },
            new SelectListItem() { Text = "Email", Value = "Email" },
    };
    public static IEnumerable<SelectListItem> ContractSortBy => new List<SelectListItem>
    {
         new SelectListItem() { Text = "Contract Start Date", Value ="ContractStartDate" },
         new SelectListItem() { Text = "Contract End Date", Value ="ContractEndDate" },
         new SelectListItem() { Text = "Contract", Value ="ContractTypeModel.Name" },
         new SelectListItem() { Text = "User Name", Value ="User.UserName" },
    };

    public static readonly Dictionary<NotificationType, NotificationsTemplate> NotificationsData = new()
    {
        {NotificationType.New_LeaveRequest ,new (){Text="New Leave requsert",URL="/Admin/LeavesType/LeaveUser" ,IsMine=false ,Color = "success",Image="/assets/images/notifications/applyrequest.jpg"}},
        {NotificationType.ChangePasswordSuccess ,new (){Text="Password Changed Succssfully.", URL="/Admin/Accounts/Profile" ,Color = "success",IsMine=true,Image="/assets/images/notifications/6146587.png" }},
        {NotificationType.New_Contract ,new (){Text="You were invited to accept a new employment contract",URL="/StaffContracts/Index"  ,Color = "success",IsMine=false,Image="/assets/images/notifications/notificationcontract.jpg"}},
        {NotificationType.Reject_LeaveRequest ,new (){Text="You Leave request Rejected by Adminstrator",URL="/StaffLeaves/Index"  ,Color = "success",IsMine=false,Image="/assets/images/notifications/rejectrequest.jpg" }},
        {NotificationType.Accept_LeaveRequest ,new (){Text="You Leave request Accepted by Adminstrator",URL="/StaffLeaves/Index"  ,Color = "success",IsMine=false,Image="/assets/images/notifications/applyrequest.jpg" }},
     };
}
#nullable disable

public class NotificationsTemplate
{
    public string Text { get; set; }
    public string URL { get; set; }
    public string Color { get; set; }
    public bool IsMine { get; set; }
    public string Image { get; set; }
}
