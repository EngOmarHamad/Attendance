namespace Attendance.Web.Controllers;

[Authorize("Staff")]
public class StaffLeavesController : Controller
{
    private readonly ILeaveUserService _leaveUserService;
    private readonly UserManager<UserModel> _userManager;
    private readonly IToastNotification _toastNotification;
    private readonly ILeaveTypeServices _leaveTypeServices;
    private readonly INotificationService _notificationServices;

    public StaffLeavesController(ILeaveUserService leaveUserService, UserManager<UserModel> userManager, IToastNotification toastNotification, ILeaveTypeServices leaveTypeServices, INotificationService notificationServices)
    {
        _leaveUserService = leaveUserService;
        _userManager = userManager;
        _toastNotification = toastNotification;
        _leaveTypeServices = leaveTypeServices;
        _notificationServices = notificationServices;
    }
    public async Task<IActionResult> Index()
    {
        return View(new AllStaffLeavesViewModel()
        {
            ListOfLeaveType = new SelectList(await _leaveTypeServices.GetAllLeaveTypes(), nameof(LeaveTypeModel.Id), nameof(LeaveTypeModel.Name)),
            SortList = new List<SelectListItem>(){
                        new SelectListItem() { Text = "Status", Value = nameof(LeaveTypeUserViewModel.Status) },
                        new SelectListItem() { Text = "Leave Name", Value = nameof(LeaveTypeUserViewModel.LeaveTypeName) },
                        new SelectListItem() { Text = "Start Leave Type", Value = nameof(LeaveTypeUserViewModel.StartLeaveType) },
                        new SelectListItem() { Text = "End Leave Type", Value = nameof(LeaveTypeUserViewModel.EndLeaveType) },

                    },
            lstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
        });
    }

    [HttpPost]
    public async Task<ActionResult> GetLeaveTypebyId(int Id)
    {
        var leavetype = await _leaveTypeServices.GetLeaveTypebyId(Id);
        return Json(leavetype);
    }

    [HttpGet("GetStaffLeaves")]
    public async Task<ActionResult> GetStaffLeaves(StaffLeavesQueryParameter QP)
    {
        var currentStaffId = _userManager.GetUserId(User);
        if (currentStaffId is null)
        {
            return NotFound();
        }

        QP.UserId = currentStaffId;

        IQueryable<LeaveTypeUserViewModel> list = (await _leaveUserService.GetFilteredDataStaffLeaves(QP))
            .Select(x => GetLeaveTypeUserViewModel(x))
            .AsQueryable();

        return Ok(list.GetPageResult(QP));

    }

    private static LeaveTypeUserViewModel GetLeaveTypeUserViewModel(LeaveUserModel leaveTypeModel)
    {
        return new LeaveTypeUserViewModel
        {
            LeaveUserId = leaveTypeModel.Id,
            EndLeaveType = leaveTypeModel.EndLeave.GetSpecialDateFromat(),
            LeaveTypeName = leaveTypeModel.LeaveType?.Name,
            Reason = leaveTypeModel.Reason,
            StartLeaveType = leaveTypeModel.StartLeave.GetSpecialDateFromat(),
            UserName = leaveTypeModel.User?.UserName,
            Status = leaveTypeModel.Status
        };
    }
    [HttpGet]
    public async Task<IActionResult> GetRemainingDays()
    {
        var userid = _userManager.GetUserId(User);
        if (userid is not null)
        {
            var leavetypeforuser = await _leaveUserService.GetAnnualUserLeaves(userid);

            var sumofcountdayleavetype = leavetypeforuser.Sum(x => x.CountDaysLeaveType);

            var leavetype = await _leaveTypeServices.GetLeaveTypeByName("Annual leave");

            if (sumofcountdayleavetype < leavetype.MaxDuration)
            {
                var diffentdays = leavetype.MaxDuration - sumofcountdayleavetype;
                return Json(diffentdays);
            }
            else
            {
                return Json("The number of days of annual leave has expired for you");
            }
        }
        return NotFound();
    }
    [HttpPost]
    public IActionResult GetEndDate(DateTime StartDate, int countDays) => Json(StartDate.AddDays(countDays).GetSpecialDateFromat());

    [HttpPost]
    //[Authorize("Staff")]
    public async Task<IActionResult> ApplyLeaveType(AddLeaveUserViewModel addLeaveUserViewModel)
    {
        if (ModelState.IsValid)
        {
            DateTime dateTime = addLeaveUserViewModel.StartLeave.AddDays((double)addLeaveUserViewModel.Numberofdays);

            var leavetypeName = (await _leaveTypeServices.GetLeaveTypebyId(addLeaveUserViewModel.LeaveTypeid)).Name;

            var userid = _userManager.GetUserId(User);
            LeaveUserModel leaveUserModel = new()
            {
                UserId = userid,
                LeaveId = addLeaveUserViewModel.LeaveTypeid,
                Reason = addLeaveUserViewModel.Reason,
                StartLeave = addLeaveUserViewModel.StartLeave,
                EndLeave = dateTime,
                CountDaysLeaveType = addLeaveUserViewModel.Numberofdays,

            };


            await _leaveUserService.CreateLeaveUser(leaveUserModel);

            await _notificationServices.NotifyPermission("Adminestrator", NotificationType.New_LeaveRequest);

            var LeaveuserViewModel = new LeaveTypeUserViewModel
            {
                LeaveUserId = addLeaveUserViewModel.LeaveTypeid,
                EndLeaveType = dateTime.GetSpecialDateFromat(),
                LeaveTypeName = leavetypeName,
                Reason = addLeaveUserViewModel.Reason,
                StartLeaveType = addLeaveUserViewModel.StartLeave.GetSpecialDateFromat(),
                Status = LeaveStatus.Pending
            };

            _toastNotification.AddSuccessToastMessage("Your request has been submitted successfully");
            return Ok(LeaveuserViewModel);
        }
        _toastNotification.AddErrorToastMessage("Something went wrong submitting the request");
        return Ok(false);


    }
    //[Authorize("Staff")]
    [HttpPost]
    public async Task<IActionResult> DeleteApplyLeaveType(int id)
    {
        bool isDeleted = await _leaveUserService.DeleteLeaveUser(id);
        return Ok(isDeleted);
    }
}
