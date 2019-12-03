using System.Collections.Generic;

namespace Zetawars.ORM
{
    public class GenericRepository<T>
    {
        public MiniORM ORM { get; set; }

        public GenericRepository()
        {
            string connectionString = "";
            this.ORM = new MiniORM(connectionString);
        }

        public virtual void Insert(T _Object)
        {
            ORM.InsertAndGetID(_Object);
        }

        public virtual List<T> GetAll()
        {
            return ORM.GetAll<T>();
        }

        public virtual T Get(int ID)
        {
            return ORM.Get<T>(whereClause: "Where ID = @ID", Params: new { ID });
        }

        public virtual void Update(T _Object)
        {
            ORM.Update(_Object);
        }

        public virtual void Delete(T _Object)
        {
            ORM.Delete(_Object);
        }

    }
}
