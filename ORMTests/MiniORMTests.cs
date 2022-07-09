using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zetawars.ORM;
namespace ORMTests
{
    public class Category2
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }





    [TestClass]
    public class MiniORMTest
    {


        public class Filter
        {
            public string Name { get; set; }
        }

        
        [Table("CustomerContact", SchemaName ="Customers")]
        public class Customer
        {
            public int CrmCustomerId { get; set; }
            public string Name { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

         [TestMethod]
        public void ReadMethod()
        {
            var orm = new MiniORM("Server=tcp:cnssql.database.windows.net,1433;Initial Catalog=TargetCrmStaging;Persist Security Info=False;User ID=csystemsdev;Password=Fort2dev{sKyp;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;workstation id=surveyService");

            var c = new Filter
            {
                Name = "Zawar"
            };
            var FirstCategory = orm.GetAll("SELECT TOP 1 * FROM Customers.CustomerContact");

            var m = FirstCategory[0].Name;

            var k = 0;

        }

        [TestMethod]
        public void InsertMethod()
        {

        }

        [TestMethod]
        public void DeleteMethod()
        {

        }


        [TestMethod]
        public void QueryTests()
        {
            var user = new User
            {
                DateOfBirth = DateTime.Parse("1994-01-04"),
                GPA = 3.10,
                ID = 10,
                LoginDate =  null,
                Name = "Zawar",
                PIN = 2291,
                Rating = 9.01,
                Salary = 100000,
                TimeStamp = DateTime.Now
            };

            string BeginTransactionQuery = QueryMaker.BeginTransQuery;
            string CommitTransactionQuery = QueryMaker.CommitTransQuery;

            string DeleteQuery = QueryMaker.DeleteQuery(user);
            string DeleteQuery2 = QueryMaker.DeleteQuery<User>(" where 1 = 1");

            var sortAndOrder = new Dictionary<string, string>();
            sortAndOrder.Add("ID", "desc");
            sortAndOrder.Add("TimeStamp", "asc");
            var PagerQuery = QueryMaker.GetPagerQueries<User>(null, sortAndOrder,0, 10);
            string InsertQuery = QueryMaker.InsertQuery(user);
            string SelectQuery = QueryMaker.SelectQuery<User>();
            string UpdateQuery = QueryMaker.UpdateQuery(user);
            return;
        }
    }
}
