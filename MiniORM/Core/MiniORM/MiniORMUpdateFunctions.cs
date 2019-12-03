using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace Zetawars.ORM
{
    public partial class MiniORM : DBCommon
    {
        #region UpdateFunctions
        public void Update<T>(T _Object, string whereClause = null, object Params = null,  string tableName = null)
        {
            ExecuteQuery(QueryMaker.UpdateQuery(_Object, whereClause, tableName), Params);
        }
        #endregion
    }
}

