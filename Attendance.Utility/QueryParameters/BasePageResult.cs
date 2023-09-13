namespace Attendance.Utility.QueryParameters
{
    public class BasePageResult<T> where T : class
    {
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public int FirstRowOnPage { get; set; }
        public int LastRowOnPage { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public IQueryable<T>? Data { get; set; }
    }

}
