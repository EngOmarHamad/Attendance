using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.DataAccess.Configurations
{
    public class ContractTypeEntityConfiguration : IEntityTypeConfiguration<ContractTypeModel>
    {
        public void Configure(EntityTypeBuilder<ContractTypeModel> builder)
        {
            builder.HasData(
                new ContractTypeModel
                {
                    Id = 1,
                    Name = "Fixed staff",
                    Description = "They have an employment contract with the organisation they work for. their contract ends on a particular date, or on completion of a specific task, eg a project."
                },
                new ContractTypeModel
                {
                    Id = 2,
                    Name = "LDC",
                    Description = "Collection Service Agreements and Transportation Agreements and the Electricity Service Agreements listed on Schedule G hereto as such agreements are in effect on the date hereof and as from time to time supplemented"
                },
                new ContractTypeModel
                {
                    Id = 3,
                    Name = "ISP-IMTD",
                    Description = "Evaluation for Determining Independent Contractor Status. Hiring departments are responsible for providing information to properly classify individuals as employees or independent contractors."
                },
                new ContractTypeModel
                {
                    Id = 4,
                    Name = "ISP-UNICC",
                    Description = "Evaluation for Determining Independent Contractor Status. Hiring departments are responsible for providing information to properly classify individuals as employees or independent contractors."
                });
        }
    }

}
