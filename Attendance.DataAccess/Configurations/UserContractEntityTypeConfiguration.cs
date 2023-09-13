namespace Attendance.DataAccess.Configurations;
public class UserContractEntityTypeConfiguration : IEntityTypeConfiguration<UserContractModel>
{
    public void Configure(EntityTypeBuilder<UserContractModel> builder)
    {
        //builder.Navigation(c => c.ContractTypeModel).AutoInclude();
        //builder.Navigation(x => x.User).AutoInclude();
    }
}
