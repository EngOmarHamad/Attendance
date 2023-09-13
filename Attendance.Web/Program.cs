using Attendance.DataAccess.Hubs;
using Attendance.Web.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();


builder.Services.AddControllersWithViews();


builder.Services.AddConfig(builder.Configuration)
    .AddAttendanceDependencyGroup()
    .AddIdentityConfiguration();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


#region ConfigreDBContext And DatabaseDeveloperPageExceptionFilter
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AttendanceDbContext>(
    options =>
    {
        options.UseLazyLoadingProxies();
        options.UseSqlServer(connectionString, options =>
        options.MigrationsAssembly("Attendance.DataAccess"));
    }).AddUnitOfWork<AttendanceDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion


builder.Services.AddSignalR();

#region AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(Program).Assembly);
#endregion


builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions
{
    ProgressBar = true,
    CloseButton = true,
    CloseDuration = true,
    HideDuration = 3
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPolices.Admin.ToString(), policyBuilder =>
    policyBuilder.RequireAssertion(
    context =>
    context.User.HasClaim("Permission", "Adminestrator")));

    options.AddPolicy(AppPolices.All.ToString(), policyBuilder =>
    policyBuilder.RequireAssertion(
    context =>
    context.User.HasClaim("Permission", "Adminestrator") ||
    context.User.HasClaim("Permission", "StaffMember")
    ));

    options.AddPolicy(AppPolices.Staff.ToString(), policyBuilder =>
    policyBuilder.RequireAssertion(
    context => context.User.HasClaim("Permission", "StaffMember"))
    );
});

//builder.Services.AddAuthentication()
//    .AddCookie(options =>
//    {
//        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//        options.Cookie.Name = "AttendanceAppCookie";
//        options.LoginPath = "/Identity/Account/Login";
//        options.LogoutPath = "/Identity/Account/Logout";
//        //options.DataProtectionProvider=
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
//        options.Cookie.HttpOnly = false;
//        options.Cookie.Expiration = TimeSpan.FromDays(7);
//        options.SlidingExpiration = true;
//    });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Error/Index/403";
    options.Cookie.Name = "AttendanceAppCookie";
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(10);
    options.Cookie.HttpOnly = false;
    options.SlidingExpiration = true;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Error/Index/{0}");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseDisabledMiddleware();

app.MapRazorPages();

app.UseNToastNotify();

app.UseHangfireDashboard("/dashboard");


app.MapHub<NotificationHub>("/NotificationHub");

app.Run();
