using Attendance.Utility.QueryParameters;

namespace Attendance.Utility.SharedMethods
{
    public static class SharedMethods
    {
        public static BasePageResult<T> GetPageResult<T>(this IQueryable<T> ListOfData, BaseQueryParameter qp) where T : class
        {

            BasePageResult<T> queryPageResult = new()
            {
                TotalCount = ListOfData.Count()
            };
            queryPageResult.TotalPages = (int)Math.Ceiling(queryPageResult.TotalCount / (double)qp.Size);
            if (queryPageResult.TotalPages < qp.CurPage)
            {
                qp.CurPage = 1;
            }
            if ((qp.CurPage - 1) > 0)
            {
                queryPageResult.PreviousPage = qp.CurPage - 1;
            }

            if ((qp.CurPage + 1) <= queryPageResult.TotalPages)
            {
                queryPageResult.NextPage = qp.CurPage + 1;
            }

            if (queryPageResult.TotalCount == 0)
            {
                queryPageResult.FirstRowOnPage = queryPageResult.LastRowOnPage = 0;
            }
            else
            {
                queryPageResult.FirstRowOnPage = ((qp.CurPage - 1) * qp.Size) + 1;
                queryPageResult.LastRowOnPage = Math.Min(qp.CurPage * qp.Size, queryPageResult.TotalCount);
            }
          ;
            queryPageResult.Data = ListOfData.Skip(qp.Size * (qp.CurPage - 1)).Take(qp.Size);
            return queryPageResult;
        }
        public static string? GetPrettyDate(this DateTime date)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(date);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff is < 0 or >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return " Just Now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "Since One minute";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("few minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "Since One Hour";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            return dayDiff == 1
                ? "منذ أمس"
                : dayDiff < 7
                ? string.Format("days ago",
                    dayDiff)
                : dayDiff < 31
                ? string.Format("weeks ago",
                    Math.Ceiling((double)dayDiff / 7))
                : "";
        }
        public static string GetSpecialDateFromat(this DateTime date, string format = "dd MMMM yyyy")
        {
            return date.ToString(format: format);
        }

        public static string GetSpecialTimeFromat(this DateTime date, string format = "h:m:s tt")
        {
            return date.ToString(format: format);
        }



    }
}
