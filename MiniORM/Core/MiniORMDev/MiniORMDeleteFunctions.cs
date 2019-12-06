using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace Zetawars.ORM
{
    public partial class MiniORMDev : DBCommon
    {
        #region DeleteFunctions
        public void Delete<T>(string whereClause, object Params, string tableName = null)
        {
            ExecuteQuery(QueryMaker.DeleteQuery<T>(whereClause), Params);
        }
        public void Delete<T>(T _Object, string whereClause = null, object Params = null, string tableName = null)
        {
            ExecuteQuery(QueryMaker.DeleteQuery(_Object, whereClause), Params);
        }
        #endregion
    }

}

