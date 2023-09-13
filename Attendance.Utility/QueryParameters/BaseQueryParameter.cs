namespace Attendance.Utility.QueryParameters
{
    public class BaseQueryParameter
    {
        const int _maxSize = 25;
        int _size = 10;
        public int CurPage { get; set; } = 1;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }

        public string SortBy { get; set; } = "Id";

        string _sortOrder = "asc";
        public string SortOrder
        {
            get { return _sortOrder; }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }


    }
}

