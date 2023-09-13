namespace Attendance.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {

        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AttendanceDbContext>();
            Dictionary<string, string> clims = new();
            clims.Add("Permission", "Admin");
            _ = AssignClaims(services: serviceProvider, "admin@gtc.edu.ps", clims);
            clims.Clear();
            clims.Add("Permission", "User");
            _ = AssignClaims(serviceProvider, "user2@gtc.edu.ps", clims);
            context.SaveChanges();
        }

        public static async Task AssignClaims(IServiceProvider services, string email, Dictionary<string, string> claims)
        {

            UserManager<UserModel>? _userManager = services.GetService<UserManager<UserModel>>() ?? throw new InvalidOperationException("UserManager is null");
            UserModel? user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            foreach (var claim in claims)
            {
                Claim nc = new(claim.Key, claim.Value);
                if (!existingUserClaims.Contains(nc))
                {
                    IdentityResult result = await _userManager.AddClaimAsync(user, nc);
                }
            }
        }
    }
}
