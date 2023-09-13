using Attendance.Utility.QueryParameters;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Attendance.DataAccess.Services;

public class AttendanceServices : IAttendanceServices
{
    private readonly AttendanceDbContext _dbContext;

    public AttendanceServices(AttendanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAttendance(AttendanceModel attendanceModel)
    {
        try
        {
            _ = await _dbContext.TblAttendance.AddAsync(attendanceModel);
            _ = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteAttendance(int Id)
    {
        try
        {
            AttendanceModel? attendance = await GetAttendancebyId(Id) ?? throw new Exception("the object is null");
            _ = _dbContext.TblAttendance.Remove(attendance);
            _ = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task EditAttendance(AttendanceModel attendanceModel)
    {
        try
        {
            _ = _dbContext.TblAttendance.Update(attendanceModel);
            _ = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    public async Task<List<AttendanceModel>> GetAllAttendance()
    {
        return await _dbContext.TblAttendance.ToListAsync();
    }

    public async Task<AttendanceModel> GetAttendancebyId(int Id)
    {
        return await _dbContext.TblAttendance.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new Exception("the object is null");
    }

    public List<AttendanceModel> GetFilteredDataAttendances(AttendanceQueryParameter QP)
    {
        IQueryable<AttendanceModel> list = _dbContext.TblAttendance;

        if (!string.IsNullOrEmpty(QP.UserId))
        {
            list = list.Where(p => p.UserId == QP.UserId);
        }
        if (QP.Status != null)
        {
            list = list.Where(p => p.AttendenceStatus == QP.Status);
        }
        if (!string.IsNullOrEmpty(QP.Day.ToString()))
        {
            list = list.Where(p => p.Day == QP.Day);
        }
        if (!string.IsNullOrEmpty(QP.DateFrom.ToString()))
        {
            list = list.Where(p => p.Day >= QP.DateFrom);
        }
        if (!string.IsNullOrEmpty(QP.DateTo.ToString()))
        {
            list = list.Where(p => p.Day <= QP.DateTo);
        }

        if (!string.IsNullOrEmpty(QP.SortBy))
        {

            if (QP.SortBy.Equals("Day", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.Day);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.Day);
            }
            else if (QP.SortBy.Equals("SignInTime", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.SignInTime);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.SignInTime);

            }
            else if (QP.SortBy.Equals("SignOutTime", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.SignOutTime);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.SignOutTime);
            }
            else if (QP.SortBy.Equals("UserName", StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderBy(p => p.User.UserName);
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    list = list.OrderByDescending(p => p.User.UserName);
            }
        }
        return list.ToList();
    }
    public async Task<DataTable> GetDataTableAttendances(string userid, DateTime? StartDate, DateTime? EndDate, int? AttendanceStatus)
    {
        DataTable dt = new();

        List<AttendanceView>? attendances = await _dbContext.AttendanceView.ToListAsync();
        Type myType = typeof(AttendanceView);

        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            // Do something with propValue
            dt.Columns.Add(prop.Name);
        }

        //dt.Columns.Add("Day");
        //dt.Columns.Add("SignInTime");
        //dt.Columns.Add("SignOutTime");
        //dt.Columns.Add("AttendenceStatus");
        //dt.Columns.Add("UserId");
        DataRow dataRow;
        if (userid is not null)
        {
            attendances = attendances.Where(x => x.UserId != null && x.UserId.Equals(userid)).ToList();
        }
        else if (StartDate is not null && EndDate is null)
        {
            attendances = attendances.Where(x => x.Day >= StartDate).ToList();
        }
        else if (StartDate is null && EndDate is not null)
        {
            attendances = attendances.Where(x => x.Day <= EndDate).ToList();
        }
        else if (StartDate is not null && EndDate is not null)
        {
            attendances = attendances.Where(x => x.Day >= StartDate && x.Day <= EndDate).ToList();
        }
        else if (AttendanceStatus is not null)
        {
            attendances = attendances.Where(x => x.AttendenceStatus.Equals(AttendanceStatus)).ToList();
        }
        foreach (var item in attendances)
        {
            dataRow = dt.NewRow();
            dataRow["Day"] = item.Day;
            dataRow["SignInTime"] = item.SignInTime;
            dataRow["SignOutTime"] = item.SignOutTime;
            dataRow["AttendenceStatus"] = Enum.GetName(typeof(AttendanceStatus), item.AttendenceStatus);
            dataRow["UserName"] = item.UserName;
            dt.Rows.Add(dataRow);
        }
        return dt;
    }
}
