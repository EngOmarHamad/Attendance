namespace Attendance.DataAccess
{
    public class AttendanceDbContext : IdentityDbContext<UserModel>
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)
        {
        }
        public DbSet<AttendanceModel> TblAttendance { get; set; }
        public DbSet<ContractTypeModel> TblContractType { get; set; }
        public DbSet<LeaveTypeModel> TblLeaveTypeModel { get; set; }
        public DbSet<LeaveUserModel> TblLeaveUserModel { get; set; }
        public DbSet<UserContractModel> TblUserContractModel { get; set; }
        public DbSet<NotificationModel> TblNotificationModel { get; set; }
        public DbSet<AttendanceView> AttendanceView { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new UserEntityTypeConfiguration().Configure(builder.Entity<UserModel>());
            new LeaveTypeEntityConfiguration().Configure(builder.Entity<LeaveTypeModel>());
            new ContractTypeEntityConfiguration().Configure(builder.Entity<ContractTypeModel>());
            new UserContractEntityTypeConfiguration().Configure(builder.Entity<UserContractModel>());
            new IdentityUserClaimEntityTypeConfiguration().Configure(builder.Entity<IdentityUserClaim<string>>());
            //builder.Entity<UserModel>().Navigation(x => x.ListOfLeaveUser).AutoInclude();
            //builder.Entity<UserModel>().Navigation(x => x.ListOfContracts).AutoInclude();
            //builder.Entity<UserModel>().Navigation(x => x.ListOfAttendance).AutoInclude();
            builder.Entity<AttendanceView>().HasNoKey().ToView("AttendanceView");
        }
    }
}