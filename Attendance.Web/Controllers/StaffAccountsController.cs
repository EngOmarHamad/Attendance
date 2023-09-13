namespace Attendance.Web.Controllers
{
    [Authorize("All")]
    public class StaffAccountsController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IMapper _mapper;
        private readonly IFileUploader _fileUploader;
        private readonly IUserStore<UserModel> _userStore;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<StaffAccountsController> _logger;
        private readonly INotificationService _notificationService;
        private readonly IToastNotification _toastNotification;
        public StaffAccountsController(UserManager<UserModel> userManager, IMapper mapper, IFileUploader fileUploader, IUserStore<UserModel> userStore, SignInManager<UserModel> signInManager, ILogger<StaffAccountsController> logger, INotificationService notificationService, IToastNotification toastNotification)
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", controllerName: "Home");
        }
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
                    await _userManager.RemoveClaimAsync(user, new Claim("UserName", value: user.UserName));
                    await _userManager.AddClaimAsync(user, new Claim("UserName", value: EditProfileSettingsViewModel.Email));
                }
                if (user.ProfileImage != null)
                {
                    await _userManager.RemoveClaimAsync(user, new Claim("ProfileImage", value: image));
                    await _userManager.AddClaimAsync(user, new Claim("ProfileImage", value: user.ProfileImage));
                }
                if (EditProfileSettingsViewModel.FirstName != null)
                {
                    await _userManager.RemoveClaimAsync(user, new Claim("FirstName", value: user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", value: EditProfileSettingsViewModel.FirstName));
                }
                if (EditProfileSettingsViewModel.FamilyName != null)
                {
                    await _userManager.RemoveClaimAsync(user, new Claim("FamilyName", value: user.FamilyName));
                    await _userManager.AddClaimAsync(user, new Claim("FamilyName", value: EditProfileSettingsViewModel.FamilyName));
                }
                user.Id = EditProfileSettingsViewModel.Id;
                user.Address = EditProfileSettingsViewModel.Address;
                user.DateOfBirth = EditProfileSettingsViewModel.DateOfBirth;
                user.Email = EditProfileSettingsViewModel.Email;
                user.FirstName = EditProfileSettingsViewModel.FirstName;
                user.FamilyName = EditProfileSettingsViewModel.FamilyName;
                user.FamilyNameAr = EditProfileSettingsViewModel.FamilyNameAr;
                user.FirstNameAr = EditProfileSettingsViewModel.FirstNameAr;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Your personal information has been modified successfully");
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                _toastNotification.AddErrorToastMessage("An error occurred modifying your personal data");
                return View(nameof(Settings), new SettingsViewModel() { EditProfileSettingsViewModel = EditProfileSettingsViewModel });
            }

            return View(nameof(Settings), new SettingsViewModel() { EditProfileSettingsViewModel = EditProfileSettingsViewModel });
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


        [HttpPost]
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

    }
}
