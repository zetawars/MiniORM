using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Zetawars.ORM
{
    public partial class QueryMaker : DBCommon
    {
        #region Public methods
        public static string DeleteQuery<T>(string whereClause, string tableName = null)
        {
            return $"Delete from {tableName ?? GetTableName<T>()} {whereClause};";
        }
        public static string DeleteQuery<T>(T _Object, string whereClause = null, string tableName = null)
        {
            var DeleteQuery = new StringBuilder();
            DeleteQuery.Append($"Delete from {tableName ?? GetTableName<T>()} ");
            if (!string.IsNullOrWhiteSpace(whereClause))
                DeleteQuery.Append($"{whereClause}"); 
            else
                DeleteQuery.Append(DefaultWhereClause(_Object));
            
            return DeleteQuery.ToString();
        }
        #endregion Public methods
    }
}

