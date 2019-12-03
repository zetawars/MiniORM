namespace Zetawars.ORM
{
    public class Pager
    {
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageNumber { get; set; }
        public int Offset { get; set; }
        public int Fetch { get; set; }
        public int TotalPages { get; set; }
    }
}

