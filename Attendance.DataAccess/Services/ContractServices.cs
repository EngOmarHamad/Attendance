using Attendance.Utility.QueryParameters;

namespace Attendance.DataAccess.Services
{
    public class ContractServices : IContractServices
    {
        private readonly AttendanceDbContext _dbContext;

        public ContractServices(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateContract(UserContractModel ContractModel)
        {
            try
            {
                await _dbContext.TblUserContractModel.AddAsync(ContractModel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteContract(int Id)
        {
            try
            {

                var contract = await GetContractbyId(Id);

                if (contract is null)
                {
                    throw new Exception("the object is null");
                }

                _dbContext.TblUserContractModel.Remove(contract);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditContract(UserContractModel ContractModel)
        {
            try
            {
                _dbContext.TblUserContractModel.Update(ContractModel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserContractModel>> GetAllContract()
        {
            return await _dbContext.TblUserContractModel.ToListAsync();
        }

        public async Task<UserContractModel> GetContractbyId(int Id)
        {
            return await _dbContext.TblUserContractModel.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public Task<List<UserContractModel>> GetFilteredDataContract(ContractQueryParameter QP)
        {
            IQueryable<UserContractModel> list = _dbContext.TblUserContractModel;
            if (QP.ContractTypeId is not null)
            {
                list = list.Where(p => p.ContractTypeId == QP.ContractTypeId);
            }
            if (!string.IsNullOrEmpty(QP.Name))
            {
                list = list.Where(p => p.UserId == QP.Name);
            }
            if (!string.IsNullOrEmpty(QP.SortBy))
            {
                if (QP.SortBy.Equals(nameof(UserContractModel.ContractStartDate), StringComparison.OrdinalIgnoreCase))
                {
                    if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderBy(p => p.ContractStartDate);
                    else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderByDescending(p => p.ContractStartDate);
                }
                if (QP.SortBy.Equals(nameof(UserContractModel.ContractEndDate), StringComparison.OrdinalIgnoreCase))
                {
                    if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderBy(p => p.ContractEndDate);
                    else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderByDescending(p => p.ContractEndDate);
                }
                if (QP.SortBy.Equals(nameof(UserContractModel.ContractTypeModel.Name), StringComparison.OrdinalIgnoreCase))
                {
                    if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderBy(p => p.ContractTypeModel.Name);
                    else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderByDescending(p => p.ContractTypeModel.Name);
                }
                if (QP.SortBy.Equals(nameof(UserContractModel.User.UserName), StringComparison.OrdinalIgnoreCase))
                {
                    if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderBy(p => p.User.UserName);
                    else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        list = list.OrderByDescending(p => p.User.UserName);
                }
            }
            return list.ToListAsync();
        }
    }
}
