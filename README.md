# MiniORM
This is a very simple ORM. Its based on ADO.NET. It ignores creating tables or manage foriegn keys. Works best when only mapping to other entites. It doesn't add complexity and keeps it just to the mapping, relying on your query writting skills.
It works best when using Repository Pattern.

You would create User.cs 

	[Table("User", Schema = "Account")] // This is optional 
	public class User
	{    
    	[Key]
    	public int ID {get;set;}
    	public string FirstName {get;set;}
    	public string LastName {get;set;}
    	public DateTime LastLoginDate{get;set;}
    	[Column("Blocked")] // Suppose that Column Name is different in database.
    	public bool Status {get;set;}
	}


this is how you will create a repository for this

    public class UserRepository
    {
        public MiniORM ORM { get; set; }
        public UserRepository()
        {
            this.ORM = new MiniORM("ConnectionStringHere");// You can also use it anyway you like.
        }

        // This is for the GetSingle User
        public User Get(int userId)
        {
            return ORM.Get<User>(userId);
        }
        // get an object without creating a class
        public dynamic(int userId)
        {
            return ORM.Get(userId);
        }

        // This is for the GetList of User
        public List<User> GetList()
        {
            return ORM.GetList<User>();
        }

        //If you want to pass a custom Query
        public List<User> GetList()
        {
            return ORM.GetList<User>("SELECT * FROM Users Where FirstName = @FirstName", new { FirstName = "Zawar" });
        }

        //get dynamic List
        public List<dynamic> GetList()
        {
            return ORM.GetList("SELECT * FROM User");
        }

        //This is how to insert
        public bool Insert(User u)
        {
            return ORM.Insert(u);
        }
		
        //This is how you update.
        public bool Update(User u)
        {
            return ORM.Update(u, "Where ID = @ID", new { ID = u.ID });
        }
    }


You can also use the QueryMaker.cs seperately to get the written querries and use them however you like.

something like

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
	  
      string SelectQuery = QueryMaker.SelectQuery<User>();
      string InsertQuery = QueryMaker.InsertQuery(user);
      string UpdateQuery = QueryMaker.UpdateQuery(user); //when updating with the Key value.
      string UpdateQuery = QueryMaker.UpdateQuery(user, "Where ID = @ID", new {ID = 1}); // Or pass a custom condition.
      
And then you can concatenate these to do a transaction. by using BeginTransQuery and CommitTransQuery. 
 
	 string query = QueryMaker.BeginTransQuery;// Starting Transaction
	 query += QueryMaker.InsertQuery(user);
	 query += QueryMaker.UpdateQuery(user);
	 query += QueryMaker.CommitTransQuery; // Committing Transaction
	 ORM.ExecuteQuery(query);
	 
	 
	 