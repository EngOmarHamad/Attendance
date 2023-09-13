namespace Attendance.DataAccess.Configurations;
public class LeaveTypeEntityConfiguration : IEntityTypeConfiguration<LeaveTypeModel>
{
    public void Configure(EntityTypeBuilder<LeaveTypeModel> builder)
    {
        builder.HasData(
            new List<LeaveTypeModel>()
            {
                new LeaveTypeModel {Id=1,Name = "Sick leave",Description = "Sick leave is paid time off from work that workers can use to stay home to address their health needs without losing pay12. Sick leave is different from paid vacation time or personal time, because it is only meant for health-related purposes1. Sick leave can be taken when a worker is ill or injured, or has to take care of a family member who is sick2."},
                new LeaveTypeModel{Id = 2,Name = "Annual leave",Description = "Annual leave is a period of paid time off granted to employees by their employer1234. It is intended to allow the employee vacation, rest and recreation2. Employees can use their annual leave for whatever they like, including holidays or just relaxing at home13. Leave is typically set out by an employment contract1",MaxDuration=15},
                new LeaveTypeModel{Id=3,Name = "Emergency leave half day or full day",Description = "Emergency Leave Day. One emergency leave day per year, charged to sick leave, will be given to an employee who is faced with a tragedy, sudden accident, etc. that would not normally be a sick leave deduction. This emergency day will be granted at the discretion of the Department Head, subject to approval by the County Board."}
            }
       );
    }
}
