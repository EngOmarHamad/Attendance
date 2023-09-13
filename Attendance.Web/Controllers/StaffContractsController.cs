using ClosedXML.Excel;

namespace Attendance.Web.Controllers
{
    [Authorize("Staff")]
    public class StaffContractsController : Controller
    {
        private readonly IContractServices _contractServices;
        private readonly IContractTypeServices _contractTypeServices;
        private readonly UserManager<UserModel> _userManager;


        public StaffContractsController(IContractServices contractServices, IContractTypeServices contractTypeServices, UserManager<UserModel> userManager)
        {
            _contractServices = contractServices;
            _contractTypeServices = contractTypeServices;
            _userManager = userManager;
        }





        // GET: StaffContractsController
        public async Task<IActionResult> Index(ContractQueryParameter QP)
        {

            var contracttype = new SelectList(await _contractTypeServices.GetAllContractTypes(), "Id", "Name");


            AllContractViewModel allContractViewModel = new AllContractViewModel()
            {
                LstSortBy = Constants.ContractSortBy,
                lstPageSize = Constants.PageSizeList.Select(i => new SelectListItem() { Text = i, Value = i }),
                lstContractType = contracttype
            };


            return View(allContractViewModel);
        }







        public async Task<IActionResult> GetContractAsync(ContractQueryParameter QP, bool ISExel)
        {
            QP.Name = _userManager.GetUserId(User);

            List<UserContractModel> list = await _contractServices.GetFilteredDataContract(QP);
            List<ContractsViewModel> contracts = new List<ContractsViewModel>();

            if (QP.Status == ContractStatus.Active)
            {
                list = list.Where(x => x.ContractStartDate <= DateTime.Now && x.ContractEndDate >= DateTime.Now).ToList();
            }

            else if (QP.Status == ContractStatus.NotActive)
            {
                list = list.Where(x => x.ContractStartDate > DateTime.Now || x.ContractEndDate < DateTime.Now).ToList();

            }

            //foreach (var x in list)
            //{

            //    contracts.Add(new ContractsViewModel()
            //    {
            //        Id = x.Id,
            //        Contract = x.ContractTypeModel.Name,
            //        ContractEndDate = x.ContractEndDate.GetSpecialDateFromat(),
            //        ContractStartDate = x.ContractStartDate.GetSpecialDateFromat(),
            //        UserId = x.UserId,
            //        UserName = x.User?.UserName,
            //        Status = status
            //    });
            //}
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





        public async Task<IActionResult> ExportExcel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contract");
            var currnetRow = 1;


            #region Header
            var props = typeof(UserContractModel).GetProperties();
            for (int i = 1; i <= props.Length; i++)
            {
                worksheet.Cell(currnetRow, i).Value = props[i - 1].Name;
            }
            #endregion

            #region Body

            foreach (var item in await _contractServices.GetAllContract())
            {
                currnetRow++;
                for (int i = 1; i <= props.Length; i++)
                {
                    worksheet.Cell(currnetRow, i).Value = typeof(UserContractModel).GetProperty(props[i - 1].Name)?.GetValue(item)?.ToString();
                }

            }
            #endregion

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedcoument.spreadsheetml.sheet", "Contracts.xlsx");



        }













        // GET: StaffContractsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: StaffContractsController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: StaffContractsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: StaffContractsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StaffContractsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StaffContractsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StaffContractsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
