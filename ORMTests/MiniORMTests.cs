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
        

         [TestMethod]
        public void ReadMethod()
        {
            var orm = new MiniORM("Data Source=(local);Initial Catalog=CVS;User ID=Zawar;Password=DreamGirl143");
            var FirstCategory = orm.Get("SELECT TOP 1 * FROM category");

            var m = FirstCategory[0].ID;

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
