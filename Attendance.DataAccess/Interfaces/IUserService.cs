namespace Attendance.DataAccess.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAll();
    }
}
