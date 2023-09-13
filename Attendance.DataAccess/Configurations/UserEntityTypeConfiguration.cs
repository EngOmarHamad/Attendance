namespace Attendance.DataAccess.Configurations;
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        PasswordHasher<UserModel> hasher = new();
        UserModel Admin = new()
        {
            Id = "7d9aefd6-d281-4abb-8734-d6bd5f044abc",
            PhoneNumber = "0597932545",
            DateOfBirth = new DateTime(2001, 11, 17),
            ProfileImage = "/assets/images/users/1.jpg",
            UserName = "Admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "Omar@gmail.com",
            NormalizedEmail = "OMAR@GMAIL.COM",
            FirstName = "Omar",
            FirstNameAr = "عمر",
            FamilyName = "Hamad",
            FamilyNameAr = "حمد",
            AboutMe = "I am an enthusiastic, self-motivated, reliable, responsible and hard working person. I am a mature team worker and adaptable to all challenging situations. I am able to work well both in a team environment as well as using own initiative. I am able to work well under pressure and adhere to strict deadlines.\r\n",
            Major = "System Administrator",
            Address = "Gaza",
            Gender = Gender.Male.ToString(),
            EmailConfirmed = true,
        };
        Admin.PasswordHash = hasher.HashPassword(Admin, password: "12345678");
        UserModel Staff = new()
        {
            Id = "5e2b9206-9a81-49fb-aa25-310f67d1afba",
            PhoneNumber = "0597070581",
            DateOfBirth = new DateTime(2002, 11, 15),
            ProfileImage = "/assets/images/users/2.jpg",
            UserName = "Nesma@gmail.com",
            NormalizedUserName = "NESMA@GMAIL.COM",
            Email = "Nesma@gmail.com",
            NormalizedEmail = "NESMA@GMAIL.COM",
            FirstName = "Nesma",
            FirstNameAr = "نسمة",
            FamilyName = "AbuShammala",
            AboutMe = "I am an enthusiastic, self-motivated, reliable, responsible and hard working person. I am a mature team worker and adaptable to all challenging situations. I am able to work well both in a team environment as well as using own initiative. I am able to work well under pressure and adhere to strict deadlines.\r\n",
            Major = "node js developer",
            FamilyNameAr = "ابو شمالة",
            Address = "Gaza",
            Gender = Gender.Female.ToString(),
            EmailConfirmed = true,

        };
        Staff.PasswordHash = hasher.HashPassword(Staff, password: "12345678");
        UserModel Staff2 = new()
        {
            Id = "b6133a44-892c-4d11-8eb6-39d7c6d433a1",
            PhoneNumber = "0597845236",
            DateOfBirth = new DateTime(2003, 08, 17),
            ProfileImage = "/assets/images/users/3.avif",
            UserName = "Hamza@gmail.com",
            NormalizedUserName = "HAMZA@GMAIL.COM",
            Email = "Hamza@gmail.com",
            NormalizedEmail = "HAMZA@GMAIL.COM",
            FirstName = "Hamza",
            FirstNameAr = "حمزة",
            FamilyName = "ALKhatib",
            FamilyNameAr = "الخطيب",
            AboutMe = "I am an enthusiastic, self-motivated, reliable, responsible and hard working person. I am a mature team worker and adaptable to all challenging situations. I am able to work well both in a team environment as well as using own initiative. I am able to work well under pressure and adhere to strict deadlines.\r\n",
            Major = "React Developer",
            Address = "Gaza",
            Gender = Gender.Male.ToString(),
            EmailConfirmed = true,
        };
        Staff2.PasswordHash = hasher.HashPassword(Staff2, password: "12345678");
        UserModel Staff3 = new()
        {
            Id = "bee2a92c-1d1f-49c2-8881-e44f256a1e65",
            PhoneNumber = "0597136584",
            DateOfBirth = new DateTime(2003, 12, 25),
            ProfileImage = "/assets/images/users/4.jpg",
            UserName = "Tariq@gmail.com",
            NormalizedUserName = "TARIQ@GMAIL.COM",
            Email = "Tariq@gmail.com",
            NormalizedEmail = "TARIQ@GMAIL.COM",
            FirstName = "Tariq",
            FirstNameAr = "طارق",
            FamilyName = "ALTeri",
            FamilyNameAr = "التري",
            Address = "Gaza",
            AboutMe = "I am an enthusiastic, self-motivated, reliable, responsible and hard working person. I am a mature team worker and adaptable to all challenging situations. I am able to work well both in a team environment as well as using own initiative. I am able to work well under pressure and adhere to strict deadlines.\r\n",
            Major = "Asp .net developr ",
            Gender = Gender.Male.ToString(),
            EmailConfirmed = true,

        };
        Staff3.PasswordHash = hasher.HashPassword(Staff3, password: "12345678");
        builder.HasData(new List<UserModel> { Admin, Staff, Staff2, Staff3 });
        //builder.Navigation(c => c.ListOfContracts).AutoInclude();

    }
}
