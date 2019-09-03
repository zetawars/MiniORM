# MiniORM
This is an ORM Developed by me. Its a work in progress.
This is an ORM. It been kept simple. It is able to perform CRUD Operations. Cannot create tables or manage foriegn keys.

The way to use it is very simple. It works best when using Repository Pattern.

SO what you can do is, create a Repo for an Entity lets say User

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
    
    public List<Dictionary<string,string>> GetList()
    {
        return DBHelper.QueryList("SELECT * FROM Users");
    }

}
