using Attendance.Utility.QueryParameters;
using System.Data;

namespace Attendance.DataAccess.Interfaces
{
    public interface IAttendanceServices
    {
        public Task CreateAttendance(AttendanceModel attendanceModel);
        public Task EditAttendance(AttendanceModel attendanceModel);
        public Task DeleteAttendance(int Id);
        public Task<AttendanceModel> GetAttendancebyId(int Id);
        public Task<List<AttendanceModel>> GetAllAttendance();
        public Task<DataTable> GetDataTableAttendances(string userid, DateTime? StartDate, DateTime? EndDate, int? AttendanceStatus);
        public List<AttendanceModel> GetFilteredDataAttendances(AttendanceQueryParameter QP);

    }
}
