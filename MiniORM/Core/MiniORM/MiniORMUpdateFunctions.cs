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
        #region UpdateFunctions
        public bool Update<T>(T _Object, string whereClause, object Params, string schemaName = null, string tableName = null)
        {
            return ExecuteQuery(QueryMaker.UpdateQuery(_Object, whereClause, schemaName, tableName), Params);
        }
        #endregion


    }

}

