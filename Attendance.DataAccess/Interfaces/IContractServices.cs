using Attendance.Utility.QueryParameters;

namespace Attendance.DataAccess.Interfaces
{
    public interface IContractServices
    {
        public Task CreateContract(UserContractModel ContractModel);
        public Task EditContract(UserContractModel ContractModel);
        public Task DeleteContract(int Id);
        public Task<UserContractModel> GetContractbyId(int Id);
        public Task<List<UserContractModel>> GetAllContract();
        public Task<List<UserContractModel>> GetFilteredDataContract(ContractQueryParameter QP);

    }
}
