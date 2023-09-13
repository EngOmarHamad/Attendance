namespace Attendance.DataAccess.Services
{
    public class ContractTypeServices : IContractTypeServices
    {


        private readonly AttendanceDbContext _dbContext;

        public ContractTypeServices(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task CreateContractType(ContractTypeModel ContractTypeModel)
        {
            try
            {
                await _dbContext.TblContractType.AddAsync(ContractTypeModel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteContractType(int Id)
        {
            try
            {

                var contracttype = await GetContractbyId(Id);

                if (contracttype is null)
                {
                    throw new Exception("the object is null");
                }

                _dbContext.TblContractType.Remove(contracttype);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditContractType(ContractTypeModel ContractTypeModel)
        {
            try
            {
                var contracttype = await _dbContext.TblContractType.FindAsync(ContractTypeModel.Id);
                if (contracttype is null)
                {
                    throw new Exception("this object is null");

                }
                contracttype.Id = ContractTypeModel.Id;
                contracttype.Name = ContractTypeModel.Name;
                contracttype.Description = ContractTypeModel.Description;

                _dbContext.TblContractType.Update(contracttype);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ContractTypeModel>> GetAllContractTypes()
        {
            return await _dbContext.TblContractType.ToListAsync();
        }

        public async Task<ContractTypeModel> GetContractbyId(int Id)
        {
            return await _dbContext.TblContractType.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
