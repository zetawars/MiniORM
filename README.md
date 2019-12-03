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
    	[Colum("Blocked")] // Suppose that ColumnName is different from the Entity class.
    	public bool Status {get;set;}
	}


this is how you will create a repository for this


	public class UserRepository
	{
    	public MiniORM ORM {get;set;}
    	public UserRepository()
    	{
          	this.ORM = new MiniORM("ConnectionStringHere");// You can also use it anyway you like.
    	}
    
    
    	// This is for the GetSingle User
    	public User Get(int UserID)
    	{
           	return ORM.Get<User>(UserID);
    	}
	
	
	
	
    	// This is for the GetList of User
    	public List<User> GetList()
    	{
         	return ORM.GetList<User>();
    	}
	
	
    
    	//If you want to pass a custom Query
    	public List<User> GetList()
    	{
        	return ORM.GetList<User>("SELECT * FROM Users Where FirstName = @FirstName", new {FirstName = "Zawar"});
    	}

    	//This is how to insert
    	public bool Inser(User u)
    	{
         	return ORM.Insert(u);
    	}
    
    
    	//This is how you update.
    	public bool Update(User u)
    	{
        	return ORM.Update(u, "Where ID = @ID", new {ID = u.ID});
    	}
    
    
    
    	// dynamically returned lists are still in progress. right now it uses a List<Dictionary<string,string>> for getting data without classes
    
    	
	//This is for getting dynamically created objects. 
	public dynamic GetList()
	{
		string query = "SELECT * FROM  WITH JOINS";
		return ORM.GetAll(query);
	}
	
	//This is for Single element using dynamic.
	public dynamic Get(int ID)
	{
	        return ORM.Get("SELECT * FROM Users");
	}
	
	}


You can also use the QueryMaker.cs seperately to get the written querries and use them however you like.

something like

      string SelectQuery = QueryMaker.SelectQuery<User>();
      string InsertQuery = QueryMaker.InsertQuery<User>();
      string UpdateQuery = QueryMaker.UpdateQuery<User>("Where ID = @ID", new {ID = 1}); // Currently I am unable to pass the Where Condition as a predicate.
      
