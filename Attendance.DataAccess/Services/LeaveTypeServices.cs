using Attendance.DataAccess.Interfaces;
using Attendance.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance.DataAccess.Services;
public class LeaveTypeServices : ILeaveTypeServices
{

    private readonly AttendanceDbContext _dbContext;

    public LeaveTypeServices(AttendanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateLeaveType(LeaveTypeModel leaveTypeModel)
    {
        try
        {
            await _dbContext.TblLeaveTypeModel.AddAsync(leaveTypeModel);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteLeaveType(int Id)
    {
        try
        {

            var leavetype = await GetLeaveTypebyId(Id) ?? throw new Exception("the object is null");
            _dbContext.TblLeaveTypeModel.Remove(leavetype);
            await _dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task EditLeaveType(LeaveTypeModel leaveTypeModel)
    {
        try
        {
            _dbContext.TblLeaveTypeModel.Update(leaveTypeModel);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteMultipe(int[] ids)
    {
        List<LeaveTypeModel> plist = new List<LeaveTypeModel>();

        foreach (var item in ids)
        {
            LeaveTypeModel p = await GetLeaveTypebyId(item) ?? throw new Exception("The item is null");
            plist.Add(p);
        }
        try
        {
            _dbContext.TblLeaveTypeModel.RemoveRange(plist);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception er)
        {
            throw new Exception(er.Message);
        }
    }


    public async Task<List<LeaveTypeModel>> GetAllLeaveTypes() => await _dbContext.TblLeaveTypeModel.ToListAsync();

    public async Task<LeaveTypeModel> GetLeaveTypebyId(int Id) => await _dbContext.TblLeaveTypeModel.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new Exception("null ref");

    public async Task<LeaveTypeModel> GetLeaveTypeByName(string name) => await _dbContext.TblLeaveTypeModel.FirstOrDefaultAsync(x => x.Name == name) ?? throw new Exception("null ref");
}
