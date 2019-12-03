using System;

namespace Zetawars.ORM
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

