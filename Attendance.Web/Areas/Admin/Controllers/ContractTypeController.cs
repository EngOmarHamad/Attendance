namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("Admin")]
    public class ContractTypeController : Controller
    {
        private readonly IContractTypeServices _contractTypeServices;

        public ContractTypeController(IContractTypeServices contractTypeServices)
        {
            _contractTypeServices = contractTypeServices;
        }
        public async Task<IActionResult> Index()
        {
            ContractViewModel contractViewModel = new()
            {
                contractTypes = await _contractTypeServices.GetAllContractTypes()
            };
            return View(contractViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(ContractTypeModel contractTypeModel)
        {
            if (ModelState.IsValid)
            {
                await _contractTypeServices.CreateContractType(contractTypeModel);
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ContractTypeModel contractTypeModel)
        {
            if (ModelState.IsValid)
            {
                await _contractTypeServices.EditContractType(contractTypeModel);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id != 0)
            {
                await _contractTypeServices.DeleteContractType(Id);
                return RedirectToAction("Index");
            }
            return NotFound();
        }


    }
}
