using Attendance.Utility.QueryParameters;

namespace Attendance.DataAccess.Interfaces;
public interface ILeaveUserService
{
    public Task<List<LeaveUserModel>> GetAllLeaveUsers();
    public Task CreateLeaveUser(LeaveUserModel leaveUserModel);
    public Task EditLeaveUser(LeaveUserModel leaveUserModel);
    public Task<bool> DeleteLeaveUser(int Id);
    public Task<LeaveUserModel> GetLeaveUserbyId(int Id);
    public Task<List<LeaveUserModel>> GetAnnualUserLeaves(string userid);
    public Task<List<LeaveUserModel>> GetLeaveUserbyUserId(string userid);
    public Task<List<LeaveUserModel>> GetFilteredDataStaffLeaves(StaffLeavesQueryParameter QP);
    public Task<List<LeaveUserModel>> GetFilteredDataLeavesUser(UserLeaveQueryParameter QP);
}
