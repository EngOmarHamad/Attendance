using Attendance.Utility.ApiResults;
using Attendance.Web.Dtos;
using ClosedXML.Excel;

namespace Attendance.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class LeavesTypeController : Controller
{
    private readonly ILeaveTypeServices _leaveTypeServices;
    private readonly ILeaveUserService _leaveUserService;
    private readonly UserManager<UserModel> _userManager;
    private readonly IToastNotification _toastNotification;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationServices;


    public LeavesTypeController(ILeaveTypeServices leaveTypeServices, ILeaveUserService leaveUserService, UserManager<UserModel> userManager, IToastNotification toastNotification, IMapper mapper, INotificationService notificationServices)
    {
        _leaveTypeServices = leaveTypeServices;
        _leaveUserService = leaveUserService;
        _userManager = userManager;
        _toastNotification = toastNotification;
        _mapper = mapper;
        _notificationServices = notificationServices;
    }


    [HttpGet]
    [Route("/GettAllLeaveType")]
    public async Task<IActionResult> GettAllLeaveType()
    {
        var list = _mapper.Map<List<LeaveTypeDto>>(await _leaveTypeServices.GetAllLeaveTypes());
        return Ok(new ApiResult<List<LeaveTypeDto>>() { Data = list, Status = true, Message = "" });
    }

    [HttpPost]
    [Route("/CreateLeaveType")]
    public async Task<ActionResult<LeaveTypeDto>> CreateLeaveType([FromForm] LeaveTypeDto leaveTypeDto)
    {
        var domainleavetypemodel = _mapper.Map<LeaveTypeModel>(leaveTypeDto);


        await _leaveTypeServices.CreateLeaveType(domainleavetypemodel);

        return Ok(new ApiResult<LeaveTypeDto>() { Data = leaveTypeDto, Status = true, Message = "Added Succesfly" });
    }


    [HttpPost]
    [Route("/EditLeaveType")]
    public async Task<ActionResult<LeaveTypeDto>> EditLeaveType([FromForm] LeaveTypeDto leaveTypeDto)
    {
        var domainleavetypemodel = _mapper.Map<LeaveTypeModel>(leaveTypeDto);

        await _leaveTypeServices.EditLeaveType(domainleavetypemodel);

        return Ok(new ApiResult<LeaveTypeDto>() { Data = leaveTypeDto, Status = true, Message = "Edit Succesfly" });
    }

    [HttpDelete]
    [Route("/DeleteLeaveType")]
    public async Task<ActionResult> DeleteLeaveType(int id)
    {
        await _leaveTypeServices.DeleteLeaveType(id);
        return Ok(new ApiResult<LeaveTypeDto>() { Status = true, Message = "Delete Succesfly" });
    }

    [HttpPost]
    [Route("/DeleteMultipleLeaveType")]
    public async Task<ActionResult<List<LeaveTypeModel>>> DeleteMultiple([FromForm] int[] ids)
    {
        await _leaveTypeServices.DeleteMultipe(ids);
        return Ok();
    }


    //[Authorize("Admin")]
    public IActionResult Index()
    {
        return View();
    }


    //[Authorize("Admin")]
    public async Task<IActionResult> LeaveUser()
    {
        List<UserModel> users = await _userManager.Users.ToListAsync();

        SelectList lstuser = new(users, nameof(UserModel.Id), nameof(UserModel.UserName));

        return View(new AllLeaveUserViewModel()
        {
            ListOfLeaveType = new SelectList(await _leaveTypeServices.GetAllLeaveTypes(), nameof(LeaveTypeModel.Id), nameof(LeaveTypeModel.Name)),
            LstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
            LstSortBy = Constants.LeavesUserSortBy,
            LstUser = lstuser
        });
    }

    public async Task<IActionResult> GetLeavesUsersAsync(UserLeaveQueryParameter QP, bool ISExel)
    {
        IQueryable<LeaveTypeUserViewModel> list = (await _leaveUserService.GetFilteredDataLeavesUser(QP))
            .Select(x => new LeaveTypeUserViewModel
            {
                EndLeaveType = x.EndLeave.GetSpecialDateFromat(),
                StartLeaveType = x.StartLeave.GetSpecialDateFromat(),
                LeaveTypeName = x.LeaveType.Name,
                Status = x.Status,
                UserName = x.User?.UserName,
                LeaveUserId = x.Id
            }).AsQueryable();
        if (ISExel)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Leave Staff");
            var currnetRow = 1;
            #region Header
            var props = typeof(LeaveTypeUserViewModel).GetProperties();
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
                    worksheet.Cell(currnetRow, i).Value = typeof(LeaveTypeUserViewModel).GetProperty(props[i - 1].Name)?.GetValue(item)?.ToString();
                }
            }
            #endregion
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedcoument.spreadsheetml.sheet", "LeavesUsers.xlsx");
        }
        return Ok(list.GetPageResult(QP));
    }


    //[Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> ApprovableRequestforLeavetype(int id)
    {
        var leaveusertype = await _leaveUserService.GetLeaveUserbyId(id);

        if (leaveusertype is null)
        {
            return NotFound();
        }


        leaveusertype.Status = LeaveStatus.Accepted;

        if (leaveusertype == null) return NotFound();
        await _leaveUserService.EditLeaveUser(leaveusertype);
        await _notificationServices.NotifyUserAsync(leaveusertype.UserId, NotificationType.Accept_LeaveRequest);

        _toastNotification.AddSuccessToastMessage("The leave request has been successfully accepted");
        return Ok(true);

    }

    [HttpPost]
    public async Task<IActionResult> RejectRequestforLeavetype(int id)
    {
        var leaveusertype = await _leaveUserService.GetLeaveUserbyId(id);

        if (leaveusertype is null)
        {
            return NotFound();
        }

        leaveusertype.Status = LeaveStatus.Rejected;

        if (leaveusertype == null) return NotFound();
        await _leaveUserService.EditLeaveUser(leaveusertype);
        await _notificationServices.NotifyUserAsync(leaveusertype.UserId, NotificationType.Reject_LeaveRequest);
        _toastNotification.AddSuccessToastMessage("The leave request has been successfully rejected");
        return Ok(true);


    }
}