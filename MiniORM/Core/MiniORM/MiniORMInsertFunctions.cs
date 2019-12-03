using System;
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
        #region InsertFunctions
        public bool Insert<T>(T _Object, string schemaName = null, string tableName = null)
        {
            return ExecuteQuery(QueryMaker.InsertQuery(_Object, schemaName, tableName));
        }
        public bool InsertAndGetID<T>(T _Object, string schemaName = null, string tableName = null)
        {
            string query = QueryMaker.InsertQuery(_Object, schemaName, tableName) + "SELECT SCOPE_IDENTITY();";
            int ID = GetScaler(query);
            foreach (var prop in _Object.GetType().GetProperties().Where(x => (Attribute.IsDefined(x, typeof(Key)))).ToList())
            {
                prop.SetValue(_Object, ID);
            }
            return true;
        }

        #endregion


    }

}

