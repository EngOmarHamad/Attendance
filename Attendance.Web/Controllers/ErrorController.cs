namespace Attendance.Web.Controllers;

[Route("[Controller]/[Action]")]
public class ErrorController : Controller
{
    [Route("{statusCode}")]
    public IActionResult Index(int statusCode) => statusCode switch
    {
        401 => View("401"),
        403 => View("403"),
        404 => View("404"),
        500 => View("500"),
        _ => View("Index"),
    };
}
