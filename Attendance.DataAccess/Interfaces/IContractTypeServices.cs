namespace Attendance.DataAccess.Interfaces
{
    public interface IContractTypeServices
    {
        public Task CreateContractType(ContractTypeModel ContractTypeModel);
        public Task EditContractType(ContractTypeModel ContractTypeModel);
        public Task DeleteContractType(int Id);
        public Task<ContractTypeModel> GetContractbyId(int Id);

        public Task<List<ContractTypeModel>> GetAllContractTypes();
    }
}
