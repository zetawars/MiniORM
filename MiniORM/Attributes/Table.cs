﻿using System;
namespace Zetawars.ORM
{
    public class Table : Attribute
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public Table(string TableName, string SchemaName = "dbo")
        {
            this.TableName = TableName;
            this.SchemaName = SchemaName;
        }
    }
}

