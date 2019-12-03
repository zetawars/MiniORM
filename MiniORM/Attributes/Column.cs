//Author : Shazaki Zetawars //
using System;

namespace MiniORM
{
    public class Column : Attribute
    {
        public string Name { get; set; }
        public Column(string name)
        {
            this.Name = name;
        }
    }

}

