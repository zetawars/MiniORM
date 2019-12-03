using Zetawars.ORM;

namespace ORMTests
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository()
        {
            string connectionString = "";
            this.ORM = new MiniORM(connectionString);
        }
    }
}
