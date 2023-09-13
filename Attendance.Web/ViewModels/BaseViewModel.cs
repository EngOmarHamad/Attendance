namespace Attendance.Web.ViewModels;

/// <summary>
/// This Model Using for shared props between all view models
/// </summary>
/// <typeparam name="T">Type of Id</typeparam>
public abstract class BaseViewModel<T> where T : class
{
    public T? Id { get; set; }
}
