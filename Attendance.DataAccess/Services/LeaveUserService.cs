using Attendance.Utility.QueryParameters;

namespace Attendance.DataAccess.Services;
public class LeaveUserService : ILeaveUserService
{
    private readonly AttendanceDbContext _dbContext;


    public LeaveUserService(AttendanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateLeaveUser(LeaveUserModel leaveUserModel)
    {
        try
        {
            await _dbContext.TblLeaveUserModel.AddAsync(leaveUserModel);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> DeleteLeaveUser(int Id)
    {
        try
        {
            var leaveuser = await GetLeaveUserbyId(Id);
            if (leaveuser != null && leaveuser.Status == LeaveStatus.Pending)
            {
                leaveuser.Status = LeaveStatus.Deleted;
                _dbContext.TblLeaveUserModel.Update(leaveuser);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task EditLeaveUser(LeaveUserModel leaveUserModel)
    {
        try
        {
            _dbContext.TblLeaveUserModel.Update(leaveUserModel);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<List<LeaveUserModel>> GetAllLeaveUsers() => await _dbContext.TblLeaveUserModel.Include(x => x.User).Include(x => x.LeaveType).ToListAsync();

    public async Task<LeaveUserModel?> GetLeaveUserbyId(int Id) => await _dbContext.TblLeaveUserModel.FirstOrDefaultAsync(x => x.Id == Id);

    public async Task<List<LeaveUserModel>> GetAnnualUserLeaves(string userid) => await _dbContext.TblLeaveUserModel.Where(x => x.UserId == userid && x.LeaveType.Name == "Annual leave").Include(x => x.LeaveType).ToListAsync();
    public async Task<List<LeaveUserModel>> GetLeaveUserbyUserId(string userid) => await _dbContext.TblLeaveUserModel.Where(x => x.UserId == userid).Include(x => x.LeaveType).ToListAsync();

    public async Task<List<LeaveUserModel>> GetFilteredDataStaffLeaves(StaffLeavesQueryParameter QP)
    {
        IQueryable<LeaveUserModel>? list = (await GetLeaveUserbyUserId(QP.UserId)).AsQueryable();
        if (QP.Status is not null)
        {
            list = list.Where(p => p.Status != null && (p.Status == QP.Status));
        }
        if (QP.StartLeaveTypeStart is not null && QP.StartLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.StartLeave >= QP.StartLeaveTypeStart && p.StartLeave <= QP.StartLeaveTypeEnd);
        }
        else if (QP.StartLeaveTypeStart is not null)
        {
            list = list.Where(p => p.StartLeave >= QP.StartLeaveTypeStart);
        }
        else if (QP.StartLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.StartLeave <= QP.StartLeaveTypeEnd);
        }
        if (QP.EndLeaveTypeStart is not null && QP.EndLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.EndLeave >= QP.EndLeaveTypeStart && p.EndLeave <= QP.EndLeaveTypeEnd);
        }
        else if (QP.EndLeaveTypeStart is not null)
        {
            list = list.Where(p => p.EndLeave >= QP.EndLeaveTypeStart);
        }
        else if (QP.EndLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.EndLeave <= QP.EndLeaveTypeEnd);
        }
        else if (QP.StaffLeaveType is not null)
        {
            var leaveid = int.Parse(QP.StaffLeaveType);
            list = list.Where(p => p.LeaveId == leaveid);
        }
        if (!string.IsNullOrEmpty(QP.SortBy))
        {
            if (QP.SortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.Status);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.Status);
            }
            else if (QP.SortBy.Equals("LeaveTypeName", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.LeaveType.Name);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.LeaveType.Name);
            }
            else if (QP.SortBy.Equals("StartLeaveType", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.StartLeave);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.StartLeave);
            }
            else if (QP.SortBy.Equals("EndLeaveType", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.EndLeave);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.EndLeave);
            }
        }
        return list.ToList();
    }
    public async Task<List<LeaveUserModel>> GetFilteredDataLeavesUser(UserLeaveQueryParameter QP)
    {
        var list = (await GetAllLeaveUsers()).AsQueryable();
        if (QP.Status is not null)
        {
            list = list.Where(p => p.Status != null && (p.Status == QP.Status));
        }
        if (QP.UserId is not null)
        {
            list = list.Where(x => x.UserId.Equals(QP.UserId));
        }
        if (QP.StartLeaveTypeStart is not null && QP.StartLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.StartLeave >= QP.StartLeaveTypeStart && p.StartLeave <= QP.StartLeaveTypeEnd);
        }
        else if (QP.StartLeaveTypeStart is not null)
        {
            list = list.Where(p => p.StartLeave >= QP.StartLeaveTypeStart);
        }
        else if (QP.StartLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.StartLeave <= QP.StartLeaveTypeEnd);
        }
        if (QP.EndLeaveTypeStart is not null && QP.EndLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.EndLeave >= QP.EndLeaveTypeStart && p.EndLeave <= QP.EndLeaveTypeEnd);
        }
        else if (QP.EndLeaveTypeStart is not null)
        {
            list = list.Where(p => p.EndLeave >= QP.EndLeaveTypeStart);
        }
        else if (QP.EndLeaveTypeEnd is not null)
        {
            list = list.Where(p => p.EndLeave <= QP.EndLeaveTypeEnd);
        }
        else if (QP.StaffLeaveType is not null)
        {
            var leaveid = int.Parse(QP.StaffLeaveType);
            list = list.Where(p => p.LeaveId == leaveid);
        }
        if (!string.IsNullOrEmpty(QP.SortBy))
        {
            if (QP.SortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.Status);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.Status);
            }
            else if (QP.SortBy.Equals("LeaveTypeName", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.LeaveType.Name);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.LeaveType.Name);
            }
            else if (QP.SortBy.Equals("StartLeaveType", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.StartLeave);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.StartLeave);
            }
            else if (QP.SortBy.Equals("EndLeaveType", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.EndLeave);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.EndLeave);
            }
        }
        return list.ToList();
    }
}

