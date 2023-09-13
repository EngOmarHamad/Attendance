namespace Attendance.Web.DependencyInjection;
public static class AttendanceServiceCollectionExtensions
{
    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AttendanceSettings>(config.GetSection("AttendanceSettings"));
        services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        return services;

    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDefaultIdentity<UserModel>()
            .AddEntityFrameworkStores<AttendanceDbContext>()
            .AddDefaultTokenProviders();


        services.Configure<IdentityOptions>(options =>
         {
             options.SignIn.RequireConfirmedPhoneNumber = false;
             options.SignIn.RequireConfirmedEmail = false;
             options.SignIn.RequireConfirmedAccount = false;

             options.Lockout.AllowedForNewUsers = true;
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
             options.Lockout.MaxFailedAccessAttempts = 5;

             options.Password.RequireNonAlphanumeric = false;
             options.Password.RequireDigit = false;
             options.Password.RequireLowercase = false;
             options.Password.RequireUppercase = false;
             options.Password.RequiredLength = 8;

             //options.Stores.ProtectPersonalData = true;
             //options.Stores.MaxLengthForKeys = 8;


             options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
             options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
             options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
             options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;

             options.User.RequireUniqueEmail = true;
             options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

         });
        return services;

    }
    public static IServiceCollection AddAttendanceDependencyGroup(this IServiceCollection services)
    {
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IFileUploader, FileSystemUploader>();
        services.AddScoped<ILeaveTypeServices, LeaveTypeServices>();
        services.AddScoped<ILeaveUserService, LeaveUserService>();
        services.AddScoped<IContractTypeServices, ContractTypeServices>();
        services.AddScoped<IContractServices, ContractServices>();
        services.AddScoped<IAttendanceServices, AttendanceServices>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAttendanceEmailService, AttendanceEmailService>();

        return services;
    }
}
