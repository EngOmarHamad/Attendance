using AspNetCore.Reporting;
using System.Data;
using System.Globalization;

namespace Attendance.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ReportController : Controller
{

    private readonly IAttendanceServices _attendanceServices;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ReportController(IAttendanceServices attendanceServices, IWebHostEnvironment webHostEnvironment)
    {
        _attendanceServices = attendanceServices;
        _webHostEnvironment = webHostEnvironment;
    }



    public async Task<IActionResult> GetReport(int reportType, string userid, DateTime? StartDate, DateTime? EndDate, int? AttendanceStatus)
    {
        DataTable? dt = await _attendanceServices.GetDataTableAttendances(userid, StartDate, EndDate, AttendanceStatus);
        string mimitype = "";
        int extenstion = 1;

        var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
        LocalReport localReport = new(path);
        localReport.AddDataSource("dsAttendance", dt);


        // For PDF
        if (reportType == 1)
        {
            var result = localReport.Execute(RenderType.Pdf, extenstion, null, mimitype);
            return File(result.MainStream, "application/pdf", "Attendance.pdf");
        }
        else
        {

            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var result = localReport.Execute(RenderType.Excel, extenstion, null, mimitype);
            return File(result.MainStream, "Application/msexcel", "Attendance.xls");
        }


    }

}
