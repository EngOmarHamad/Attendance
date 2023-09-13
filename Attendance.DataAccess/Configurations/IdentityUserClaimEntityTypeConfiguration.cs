namespace Attendance.DataAccess.Configurations;
public class IdentityUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        int id = 1;
        builder.HasData(
            new List<IdentityUserClaim<string>>(){
                //first User Admin
                new IdentityUserClaim<string>{Id = id++,UserId = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",ClaimType = "Permission",ClaimValue = "Adminestrator"},
                new IdentityUserClaim<string>{Id = id++,UserId = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",ClaimType = "ProfileImage",ClaimValue = "/assets/images/users/1.jpg"},
                new IdentityUserClaim<string>{Id = id++,UserId = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",ClaimType = "FirstName",ClaimValue = "Omar"},
                new IdentityUserClaim<string>{Id = id++,UserId = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",ClaimType = "FamilyName",ClaimValue = "Hamad"},
                new IdentityUserClaim<string>{Id = id++,UserId = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",ClaimType = "UserName",ClaimValue = "Omar@gmail.com"},
               
                //Second User Nesma
                new IdentityUserClaim<string>{Id = id++,UserId = "5e2b9206-9a81-49fb-aa25-310f67d1afba",ClaimType = "Permission",ClaimValue = "StaffMember"},
                new IdentityUserClaim<string>{Id = id++,UserId = "5e2b9206-9a81-49fb-aa25-310f67d1afba",ClaimType = "ProfileImage",ClaimValue = "/assets/images/users/2.jpg"},
                new IdentityUserClaim<string>{Id = id++,UserId = "5e2b9206-9a81-49fb-aa25-310f67d1afba",ClaimType = "FirstName",ClaimValue = "Nesma"},
                new IdentityUserClaim<string>{Id = id++,UserId = "5e2b9206-9a81-49fb-aa25-310f67d1afba",ClaimType = "FamilyName",ClaimValue = "AbuShammala"},
                new IdentityUserClaim<string>{Id = id++,UserId = "5e2b9206-9a81-49fb-aa25-310f67d1afba",ClaimType = "UserName",ClaimValue = "Nesma@gmail.com"},


                //Second User Hamza
                new IdentityUserClaim<string>{Id = id ++,UserId = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",ClaimType = "Permission",ClaimValue = "StaffMember"},
                new IdentityUserClaim<string>{Id = id++,UserId = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",ClaimType = "ProfileImage",ClaimValue = "/assets/images/users/3.avif"},
                new IdentityUserClaim<string>{Id = id++,UserId = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",ClaimType = "FirstName",ClaimValue = "Hamza"},
                new IdentityUserClaim<string>{Id = id++,UserId = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",ClaimType = "FamilyName",ClaimValue = "ALKhatib"},
                new IdentityUserClaim<string>{Id = id++,UserId = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",ClaimType = "UserName",ClaimValue = "Hamza@gmail.com"},

                //Second User Tariq
                new IdentityUserClaim<string>{Id = id ++,UserId = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",ClaimType = "Permission",ClaimValue = "StaffMember"},
                new IdentityUserClaim<string>{Id = id++,UserId = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",ClaimType = "ProfileImage",ClaimValue = "/assets/images/users/4.jpg"},
                new IdentityUserClaim<string>{Id = id++,UserId = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",ClaimType = "FirstName",ClaimValue = "Tariq"},
                new IdentityUserClaim<string>{Id = id++,UserId = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",ClaimType = "FamilyName",ClaimValue = "ALTeri"},
                new IdentityUserClaim<string>{Id = id++,UserId = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",ClaimType = "UserName",ClaimValue = "Tariq@gmail.com"},
            });

    }
}

