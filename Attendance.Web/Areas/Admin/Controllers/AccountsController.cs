using ClosedXML.Excel;

namespace Attendance.Web.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize("Admin")]
public class AccountsController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IMapper _mapper;
    private readonly IFileUploader _fileUploader;
    private readonly IUserStore<UserModel> _userStore;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly ILogger<AccountsController> _logger;
    private readonly INotificationService _notificationService;
    private readonly IToastNotification _toastNotification;
    public AccountsController(UserManager<UserModel> userManager, IMapper mapper, IFileUploader fileUploader, IUserStore<UserModel> userStore, SignInManager<UserModel> signInManager, ILogger<AccountsController> logger, INotificationService notificationService, IToastNotification toastNotification)
    {
        _userManager = userManager;
        _mapper = mapper;
        _fileUploader = fileUploader;
        _userStore = userStore;
        _signInManager = signInManager;
        _logger = logger;
        _notificationService = notificationService;
        _toastNotification = toastNotification;
    }
    [Authorize("Admin")]
    public async Task<IActionResult> Settings(bool isnotification)
    {
        if (isnotification)
        {
            ViewBag.NotificationTab = true;
        }
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }
        return View(new SettingsViewModel()
        {
            EditProfileSettingsViewModel = new EditProfileSettingsViewModel()
            {
                AboutMe = user.AboutMe,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FamilyName = user.FamilyName,
                FamilyNameAr = user.FamilyNameAr,
                FirstName = user.FirstName,
                Gender = user.Gender == "Male" ? 0 : 1,
                FirstNameAr = user.FirstNameAr,
                Status = user.Status,
                ProfileImage = user.ProfileImage,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                Major = user.Major,
                JoinedDate = user.JoinedDate.ToString(),
            }
        });
    }
    [HttpPost]
    [Authorize("Admin")]
    public async Task<IActionResult> EditProfile(EditProfileSettingsViewModel EditProfileSettingsViewModel)
    {
        if (ModelState.IsValid)
        {
            UserModel? user = await _userManager.FindByIdAsync(EditProfileSettingsViewModel.Id);
            if (user is null)
            {
                return NotFound();
            }
            string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (user.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
                if (!setPhoneResult.Succeeded) { return View(EditProfileSettingsViewModel); }
                else
                {
                    foreach (IdentityError item in setPhoneResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(nameof(Settings), new SettingsViewModel() { EditProfileSettingsViewModel = EditProfileSettingsViewModel });
                }

            }

            var image = user.ProfileImage;
            user.ProfileImage = await _fileUploader.Edit(user.ProfileImage, EditProfileSettingsViewModel.Image, "Images\\Users");

            if (EditProfileSettingsViewModel.Email != null)
            {
                var isreplaced = await _userManager.ReplaceClaimAsync(user, new Claim("UserName", value: user.UserName), new Claim("UserName", value: EditProfileSettingsViewModel.Email));
            }
            if (EditProfileSettingsViewModel.ProfileImage != null)
            {
                var isreplaced2 = await _userManager.ReplaceClaimAsync(user, new Claim("ProfileImage", value: image), new Claim("ProfileImage", value: EditProfileSettingsViewModel.ProfileImage));
            }
            if (EditProfileSettingsViewModel.FirstName != null)
            {
                var isreplaced3 = await _userManager.ReplaceClaimAsync(user, new Claim("FirstName", value: user.FirstName), new Claim("FirstName", value: EditProfileSettingsViewModel.FirstName));
            }
            if (EditProfileSettingsViewModel.FamilyName != null)
            {
                var isreplaced4 = await _userManager.ReplaceClaimAsync(user, new Claim("FamilyName", value: user.FamilyName), new Claim("FamilyName", value: EditProfileSettingsViewModel.FamilyName));
            }
            user.Id = EditProfileSettingsViewModel.Id;
            user.Address = EditProfileSettingsViewModel.Address;
            user.DateOfBirth = EditProfileSettingsViewModel.DateOfBirth;
            user.FirstName = EditProfileSettingsViewModel.FirstName;
            user.FamilyName = EditProfileSettingsViewModel.FamilyName;
            user.FamilyNameAr = EditProfileSettingsViewModel.FamilyNameAr;
            user.FirstNameAr = EditProfileSettingsViewModel.FirstNameAr;




            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {


                return RedirectToAction("Profile");
            }
            else
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(nameof(Settings), new SettingsViewModel() { EditProfileSettingsViewModel = EditProfileSettingsViewModel });
        }

        return View(nameof(Settings), new SettingsViewModel() { EditProfileSettingsViewModel = EditProfileSettingsViewModel });
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    [Authorize("Admin")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var result = await _userManager.ChangePasswordAsync(user,
                changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            await _signInManager.RefreshSignInAsync(user);
            _toastNotification.AddSuccessToastMessage("Password changed successfully");
            await _notificationService.NotifyUserAsync(user.Id, NotificationType.ChangePasswordSuccess);
            return RedirectToAction("Profile", new { userid = user.Id });
        }
        return View();
    }

    [HttpGet]
    [Authorize("Admin")]
    public IActionResult Index()
    {
        return View(new AllUsersViewModel()
        {
            SortList = Constants.UserSortBy,
            lstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
        });
    }

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(UserQueryParameter QP, bool ISExel)
    {
        IList<UserModel> users = !QP.OnlyStaff ?? true ? await _userManager.Users.ToListAsync() : await _userManager.GetUsersForClaimAsync(new Claim("Permission", AppClaimTypes.StaffMember.ToString()));
        List<UserViewModel>? listOfUserViewModel = new();
        foreach (UserModel item in users)
        {
            listOfUserViewModel.Add(GetUserViewModel(item));
        };
        IQueryable<UserViewModel> list = listOfUserViewModel.AsQueryable();
        //var list = _mapper.Map<List<UserViewModel>>(users).AsQueryable();
        //if (!string.IsNullOrEmpty(QP.SearchTearm))
        //{
        //    list = list.Where(p =>
        //    (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())) ||
        //    (p.FirstNameAr != null && p.FirstNameAr.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())) ||
        //    (p.Address != null && p.Address.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())) ||
        //    (p.FamilyNameAr != null && p.FamilyNameAr.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())) ||
        //    (p.Email != null && p.Email.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())) ||
        //    (p.FamilyName != null && p.FamilyName.Trim().ToLower().Contains(QP.SearchTearm.Trim().ToLower())));
        //}

        if (!string.IsNullOrEmpty(QP.FirstName))
        {
            list = list.Where(p => p.FirstName != null && p.FirstName.Trim().ToLower().Contains(QP.FirstName.Trim().ToLower()));
        }
        if (!string.IsNullOrEmpty(QP.FamilyName))
        {
            list = list.Where(p => p.FamilyName != null && p.FamilyName.Trim().ToLower().Contains(QP.FamilyName.Trim().ToLower()));
        }
        if (!string.IsNullOrEmpty(QP.FirstNameAr))
        {
            list = list.Where(p => p.FirstNameAr != null && p.FirstNameAr.Trim().ToLower().Contains(QP.FirstNameAr.Trim().ToLower()));
        }
        if (!string.IsNullOrEmpty(QP.FamilyNameAr))
        {
            list = list.Where(p => p.FamilyNameAr != null && p.FamilyNameAr.Trim().ToLower().Contains(QP.FamilyNameAr.Trim().ToLower()));
        }
        if (!string.IsNullOrEmpty(QP.Address))
        {
            list = list.Where(p => p.Address != null && p.Address.Trim().ToLower().Contains(QP.Address.Trim().ToLower()));
        }
        if (!string.IsNullOrEmpty(QP.Email))
        {
            list = list.Where(p => p.Email != null && p.Email.Trim().ToLower().Contains(QP.Email.Trim().ToLower()));
        }
        if (QP.Status is not null)
        {
            list = list.Where(p => p.Status != null && p.Status == (QP.Status == 0 ? UserStatus.Active.ToString() : UserStatus.Inactive.ToString()));
        }
        if (QP.Gender is not null)
        {
            list = list.Where(p => p.Gender != null && p.Gender == QP.Gender);
        }
        if (!string.IsNullOrEmpty(QP.SortBy))
        {
            if (QP.SortBy.Equals(nameof(UserViewModel.FirstName), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.FirstName);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.FirstName);
                }
            }
            else if (QP.SortBy.Equals(nameof(UserViewModel.FamilyName), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.FamilyName);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.FamilyName);
                }
            }
            else if (QP.SortBy.Equals(nameof(UserViewModel.FamilyNameAr), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.FamilyNameAr);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.FamilyNameAr);
                }
            }
            else if (QP.SortBy.Equals(nameof(UserViewModel.FirstNameAr), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.FirstNameAr);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.FamilyName);
                }
            }
            else if (QP.SortBy.Equals(nameof(UserViewModel.DateOfBirth), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.DateOfBirth);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.DateOfBirth);
                }
            }
            else if (QP.SortBy.Equals(nameof(UserViewModel.Email), StringComparison.OrdinalIgnoreCase))
            {
                if (QP.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderBy(p => p.Email);
                }
                else if (QP.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.OrderByDescending(p => p.Email);
                }
            }
        }

        if (ISExel)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currnetRow = 1;
            #region Header
            var props = typeof(UserViewModel).GetProperties();
            for (int i = 1; i <= props.Length; i++)
            {
                worksheet.Cell(currnetRow, i).Value = props[i - 1].Name;
            }
            #endregion
            #region Body
            foreach (var item in list)
            {
                currnetRow++;
                for (int i = 1; i <= props.Length; i++)
                {
                    worksheet.Cell(currnetRow, i).Value = typeof(UserViewModel).GetProperty(props[i - 1].Name)?.GetValue(item)?.ToString();
                }
            }
            #endregion
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedcoument.spreadsheetml.sheet", "StaffMember.xlsx");
        }

        return Ok(list.GetPageResult(QP));
    }
    [Authorize("Admin")]
    public IActionResult AddStaff()
    {
        return View(new AddUserViewModel());
    }
    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> AddStaff(AddUserViewModel addUserViewModel)
    {
        if (!ModelState.IsValid) { return View(addUserViewModel); }
        UserModel user = _mapper.Map<UserModel>(addUserViewModel);
        user.Id = Guid.NewGuid().ToString();
        await _userStore.SetUserNameAsync(user, addUserViewModel.Email, CancellationToken.None);
        user.Gender = ((Gender)(addUserViewModel.Gender ?? 0)).ToString();
        user.Status = UserStatus.Active.ToString();
        user.ProfileImage = await _fileUploader.UploadAsync(addUserViewModel.Image, "Images\\Users");
        Claim userClaim = new("Permission", AppClaimTypes.StaffMember.ToString());
        IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);
        if (result.Succeeded)
        {
            List<Claim> claims = new()
                    {   userClaim,
                        new Claim("UserName",value: user.UserName ),
                        new Claim("ProfileImage",value: user.ProfileImage ),
                        new Claim("FirstName",user.FirstName),
                        new Claim("FamilyName",user.FamilyName),
                    };
            _ = await _userManager.AddClaimsAsync(user, claims);
        }
        else
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(addUserViewModel);
        }
        return RedirectToAction("Index", "Accounts");
    }
    [HttpGet]
    [Authorize("Admin")]
    public async Task<IActionResult> EditStaff(string id)
    {
        UserModel? user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }
        EditUserViewModel userviewmodel = _mapper.Map<EditUserViewModel>(user);

        return View(userviewmodel);
    }
    [HttpPost]
    [Authorize("Admin")]
    public async Task<IActionResult> EditStaff(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            UserModel? user = await _userManager.FindByIdAsync(model.Id);
            string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (user.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
                if (!setPhoneResult.Succeeded) { return View(model); }
                else
                {
                    foreach (IdentityError item in setPhoneResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }

            }

            var image = user.ProfileImage;
            user.ProfileImage = await _fileUploader.Edit(user.ProfileImage, model.Image, "Images\\Users");

            var isreplaced = await _userManager.ReplaceClaimAsync(user, new Claim("UserName", value: user.UserName), new Claim("UserName", value: model.Email));
            var isreplaced2 = await _userManager.ReplaceClaimAsync(user, new Claim("ProfileImage", value: user.ProfileImage), new Claim("ProfileImage", value: user.ProfileImage));
            var isreplaced3 = await _userManager.ReplaceClaimAsync(user, new Claim("FirstName", value: user.FirstName), new Claim("FirstName", value: model.FirstName));
            var isreplaced4 = await _userManager.ReplaceClaimAsync(user, new Claim("FamilyName", value: user.FamilyName), new Claim("FamilyName", value: model.FamilyName));

            user.Id = model.Id;
            user.Address = model.Address;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.FamilyName = model.FamilyName;
            user.FamilyNameAr = model.FamilyNameAr;
            user.FirstNameAr = model.FirstNameAr;



            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Accounts");
            }
            else
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);

        }

        return View(model);
    }
    [Authorize("Admin")]
    public async Task<IActionResult> GetUserProfile(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        UserContractModel? CurrentuserContractModel = user.ListOfContracts?.Where(i => DateTime.Now >= i.ContractStartDate && DateTime.Now <= i.ContractEndDate).FirstOrDefault();

        UserViewModel userViewModel = new()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            AboutMe = user.AboutMe,
            Address = user.Address,
            DateOfBirth = user.JoinedDate?.GetSpecialDateFromat(),
            CurrentContractName = CurrentuserContractModel?.ContractTypeModel?.Name,
            HasContract = CurrentuserContractModel != null,
            Gender = user.Gender == "Male" ? 0 : 1,
            FamilyName = user.FamilyName,
            FirstNameAr = user.FirstNameAr,
            FamilyNameAr = user.FamilyNameAr,
            JoinedDate = user.JoinedDate?.GetSpecialDateFromat(),
            Id = user.Id,
            Major = user.Major,
            PhoneNumber = user.PhoneNumber,
            ProfileImage = user.ProfileImage,
            Status = user.Status,
        };

        return View(nameof(Profile), userViewModel);
    }

    [HttpPost("/ValidateEmail")]

    public async Task<IActionResult> ValidateEmailAsync(string email, string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var user2 = await _userManager.FindByEmailAsync(email);
        if (user is not null && user2 is not null)
        {
            if (user.Email == user2.Email)
            {
                return Ok(new { success = true, message = $"Valid Email" });

            }
        }
        if (user2 is not null)
        {
            return Ok(new { success = false, message = $"this {email} is already token" });

        }
        else
        {
            return Ok(new { success = true, message = $"Valid Email" });

        }
    }

    public async Task<IActionResult> ActivateUser(string id)
    {
        UserModel? user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        user.Status = user.Status.Equals(UserStatus.Inactive.ToString()) ? user.Status = UserStatus.Active.ToString()
             : user.Status = UserStatus.Inactive.ToString();

        IdentityResult result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Accounts");
        }
        else
        {
            foreach (IdentityError item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

        return View();
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return NotFound();
        }
        UserContractModel? CurrentuserContractModel = user.ListOfContracts?.Where(i => DateTime.Now >= i.ContractStartDate && DateTime.Now <= i.ContractEndDate).FirstOrDefault();

        UserViewModel userViewModel = new()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            AboutMe = user.AboutMe,
            Address = user.Address,
            DateOfBirth = user.JoinedDate?.GetSpecialDateFromat(),
            CurrentContractName = CurrentuserContractModel?.ContractTypeModel?.Name,
            HasContract = CurrentuserContractModel != null,
            Gender = user.Gender == "Male" ? 0 : 1,
            FamilyName = user.FamilyName,
            FirstNameAr = user.FirstNameAr,
            FamilyNameAr = user.FamilyNameAr,
            JoinedDate = user.JoinedDate?.GetSpecialDateFromat(),
            Id = user.Id,
            Major = user.Major,
            PhoneNumber = user.PhoneNumber,
            ProfileImage = user.ProfileImage,
            Status = user.Status,
        };

        return View(userViewModel);
    }

    public async Task<IActionResult> GetPartialUserDetails(string id)
    {
        UserModel? user = await _userManager.FindByIdAsync(id);

        return user is null ? NotFound() : PartialView("PartialUserDetails", GetUserViewModel(user));
    }

    private static UserViewModel GetUserViewModel(UserModel user)
    {

        UserContractModel? CurrentuserContractModel = user.ListOfContracts?.Where(i => DateTime.Now >= i.ContractStartDate && DateTime.Now <= i.ContractEndDate).FirstOrDefault();
        return new()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            AboutMe = user.AboutMe,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth?.GetSpecialDateFromat(),
            CurrentContractName = CurrentuserContractModel?.ContractTypeModel?.Name,
            HasContract = CurrentuserContractModel != null,
            Gender = user.Gender == "Male" ? 0 : 1,
            FamilyName = user.FamilyName,
            FirstNameAr = user.FirstNameAr,
            FamilyNameAr = user.FamilyNameAr,
            JoinedDate = user.JoinedDate?.GetSpecialDateFromat(),
            Id = user.Id,
            Major = user.Major,
            PhoneNumber = user.PhoneNumber,
            ProfileImage = user.ProfileImage,
            Status = user.Status,
        };
    }



}
