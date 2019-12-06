using System;
using System.Linq;
namespace Zetawars.ORM
{
    public partial class MiniORMDev : DBCommon
    {
        #region InsertFunctions
        public void Insert<T>(T _Object, string tableName = null)
        {
            ExecuteQuery(QueryMaker.InsertQuery(_Object, tableName));
        }
        public void InsertAndGetID<T>(T _Object, string schemaName = null, string tableName = null)
        {
            string query = QueryMaker.InsertQuery(_Object, tableName) + "SELECT SCOPE_IDENTITY();";
            int ID = GetScaler(query);
            var PrimaryKey = GetKeyProperty<T>();
            PrimaryKey.SetValue(_Object, ID);
        }
        #endregion
    }
}

