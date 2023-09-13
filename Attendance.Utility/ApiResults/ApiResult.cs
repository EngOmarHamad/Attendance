namespace Attendance.Utility.ApiResults;
public class ApiResult<T> where T : class
{
    public T? Data { get; set; }
    public bool? Status { get; set; } = true;
    public string? Message { get; set; } = "Sucess";
}
