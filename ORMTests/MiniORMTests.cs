using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zetawars.ORM;
namespace ORMTests
{
    [TestClass]
    public class MiniORMTest
    {

        public UserRepository UserRepo = new UserRepository();


        [TestMethod]
        public void ReadMethod()
        {
            var users = UserRepo.GetAll();
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
