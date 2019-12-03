using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
//Author : Shazaki Zetawars //
namespace MiniORM
{


    public partial class MiniORM : DB_Common
    {
        #region DeleteFunctions
        public bool Delete<T>(string whereClause, object Params, string schemaName = null, string tableName = null)
        {
            return ExecuteQuery(QueryMaker.DeleteQuery<T>(whereClause), Params);
        }
        #endregion
    }

}

