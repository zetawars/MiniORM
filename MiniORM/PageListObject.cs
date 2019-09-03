using System.Collections.Generic;
//Author : Shazaki Zetawars //
namespace System
{
    public class PageListObject<T>
    {
        public Pager pager { get; set; }
        public List<T> Results { get; set; }
    }

}

