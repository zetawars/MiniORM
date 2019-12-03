using System.Collections.Generic;
namespace Zetawars.ORM
{
    public class PageListObject<T>
    {
        public Pager pager { get; set; }
        public List<T> Results { get; set; }
    }
}

