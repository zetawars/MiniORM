using System;
using System.Linq;

namespace MiniORMTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //MiniORM ORM = new MiniORM("TempConnectionString");

            //var user = new User();
            //user.FirstName = "Zawar";
            //user.LastName = "Mahmood";
            //user.DateOfBirth = DateTime.Parse("1994-01-04");
            //user.ID = 0;


            ////ORM.Insert(user);
            ////ORM.Update(user, "Where ID = @ID", new {  user.ID });

            //Console.WriteLine(QueryMaker.InsertQuery(user));
            //Console.WriteLine(QueryMaker.UpdateQuery(user, "Where ID = 1"));


            int[] arr = new int[] {1,4,5,7, 10 };

            for (int i = 0; i < 10; i++)
            {
                if (!arr.Contains(i))
                {
                    Console.WriteLine(i);
                }
            }




            string name = "Faisal Ayub";
            name = name.ToLower();
            var k = name.ToCharArray().GroupBy(x => x).Select(x => new { x.Key, Count = x.Count() }).Where(x=>x.Count> 1).ToDictionary(x => x.Key, y => y.Count);


            foreach (var item in k)
            {
                Console.WriteLine(item.Key + " : " + item.Value);

            }

            Console.ReadLine();
        }
    }


    [Table("User", SchemaName ="Users")]
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
