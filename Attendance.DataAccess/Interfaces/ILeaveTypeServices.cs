namespace Attendance.DataAccess.Interfaces;
public interface ILeaveTypeServices
{

    public Task CreateLeaveType(LeaveTypeModel leaveTypeModel);
    public Task EditLeaveType(LeaveTypeModel leaveTypeModel);
    public Task DeleteLeaveType(int Id);
    public Task<LeaveTypeModel> GetLeaveTypebyId(int Id);

    public Task<List<LeaveTypeModel>> GetAllLeaveTypes();

    public Task DeleteMultipe(int[] ids);

    public Task<LeaveTypeModel> GetLeaveTypeByName(string name);


}
