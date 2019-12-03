using System;
using System.Collections.Generic;
using System.Text;
using Zetawars.ORM;

namespace ORMTests
{
    public class User
    {
        [PrimaryKey]
        public int ID { get; set; }

        // NOT NULL Integer
        public int PIN { get; set; }

        //Nullable Integer
        public int? Salary { get; set; }

        //Not Nullable double
        public double GPA { get; set; }

        //Nullable Doubles
        public double? Rating { get; set; }

        // Strings
        public string Name { get; set; }
        
        //Nullable Date
        public DateTime? LoginDate { get; set; }

        // Not Nullable Date with Default value in DB.
        public DateTime TimeStamp { get; set; }

        // Not Nullable Date
        public DateTime DateOfBirth { get; set; }
    }
}
