using ClosedXML.Excel;

namespace Attendance.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContractController : Controller
    {
        private readonly IContractServices _contractServices;
        private readonly IContractTypeServices _contractTypeServices;
        private readonly UserManager<UserModel> _userManager;
        private readonly INotificationService _notificationServices;
        private readonly IAttendanceEmailService _attendanceEmailService;

        public ContractController(IContractServices contractServices, IContractTypeServices contractTypeServices, UserManager<UserModel> userManager, INotificationService notificationServices, IAttendanceEmailService attendanceEmailService, UserManager<UserModel> usermananger)
        {
            _contractServices = contractServices;
            _contractTypeServices = contractTypeServices;
            _userManager = userManager;
            _notificationServices = notificationServices;
            _attendanceEmailService = attendanceEmailService;
        }

        public async Task<IActionResult> Index(ContractQueryParameter QP)
        {
            var users = await _userManager.Users.ToListAsync();

            var lstuser = new SelectList(users, "Id", "UserName");
            var contracttype = new SelectList(await _contractTypeServices.GetAllContractTypes(), "Id", "Name");


            AllContractViewModel allContractViewModel = new AllContractViewModel()
            {
                LstSortBy = Constants.ContractSortBy,
                lstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
                lstContractType = contracttype,
                lstUser = lstuser,
            };


            return View(allContractViewModel);
        }



        public async Task<IActionResult> GetContractAsync(ContractQueryParameter QP, bool ISExel)
        {
            List<UserContractModel> list = await _contractServices.GetFilteredDataContract(QP);
            List<ContractsViewModel> contracts = new();

            if (QP.Status == ContractStatus.Active)
            {
                list = list.Where(x => x.ContractStartDate <= DateTime.Now && x.ContractEndDate >= DateTime.Now).ToList();
            }

            else if (QP.Status == ContractStatus.NotActive)
            {
                list = list.Where(x => x.ContractStartDate > DateTime.Now || x.ContractEndDate < DateTime.Now).ToList();

            }

            contracts = list.Select(x => new ContractsViewModel()
            {
                Id = x.Id,
                Contract = x.ContractTypeModel?.Name,
                ContractEndDate = x.ContractEndDate.GetSpecialDateFromat(),
                ContractStartDate = x.ContractStartDate.GetSpecialDateFromat(),
                UserId = x.UserId,
                UserName = x.User?.UserName,
                Status = (x.ContractStartDate <= DateTime.Now && x.ContractEndDate >= DateTime.Now) ? "Active" : "Not Active"
            }).ToList();

            if (ISExel)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Contract");
                var currnetRow = 1;
                #region Header
                var props = typeof(ContractsViewModel).GetProperties();
                for (int i = 1; i <= props.Length; i++)
                {
                    worksheet.Cell(currnetRow, i).Value = props[i - 1].Name;
                }
                #endregion
                #region Body
                foreach (var item in contracts)
                {
                    currnetRow++;
                    for (int i = 1; i <= props.Length; i++)
                    {
                        worksheet.Cell(currnetRow, i).Value = typeof(ContractsViewModel).GetProperty(props[i - 1].Name)?.GetValue(item)?.ToString();
                    }
                }
                #endregion
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedcoument.spreadsheetml.sheet", "Contracts.xlsx");
            }

            return Ok(contracts.AsQueryable().GetPageResult(QP));
        }
        public async Task<IActionResult> Add(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                var contracttype = await _contractTypeServices.GetAllContractTypes();
                ViewBag.ContractType = new SelectList(contracttype, "Id", "Name");
                UserContractModel userContractModel = new();
                userContractModel.UserId = userID;
                return View(userContractModel);
            }
            return NotFound();

        }



        [HttpPost]
        public async Task<IActionResult> Add(UserContractModel userContractModel)
        {
            if (ModelState.IsValid)
            {
                UserContractModel? CurrentuserContractModel = (await _contractServices.GetAllContract()).Where(i => DateTime.Now >= i.ContractStartDate && DateTime.Now <= i.ContractEndDate).FirstOrDefault();
                if (CurrentuserContractModel == null)
                {
                    await _contractServices.CreateContract(userContractModel);

                    await _notificationServices.NotifyUserAsync(userContractModel.UserId, NotificationType.New_Contract);

                    var user = await _userManager.FindByIdAsync(userContractModel.UserId);

                    if (user is null)
                    {
                        return NotFound();
                    }
                    if (user.Email != null)
                    {
                        BackgroundJob.Enqueue(() => _attendanceEmailService.SendEmailAsync(user.Email, "add new contract", ""));
                    }
                }

                return RedirectToAction("Index");

            }
            return NotFound();
        }


        public async Task<IActionResult> Edit(int Id)
        {
            if (Id != 0 && Id != null)
            {
                var contracttype = await _contractTypeServices.GetAllContractTypes();
                ViewBag.ContractType = new SelectList(contracttype, "Id", "Name");
                UserContractModel userContractModel = await _contractServices.GetContractbyId(Id);
                return View(userContractModel);

            }
            return NotFound();

        }



        [HttpPost]
        public async Task<IActionResult> Edit(UserContractModel userContractModel)
        {
            if (ModelState.IsValid)
            {

                await _contractServices.EditContract(userContractModel);
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        [HttpPost]

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id != 0)
            {
                await _contractServices.DeleteContract(Id);
                return RedirectToAction("Index");
            }
            return NotFound();
        }



    }
}
