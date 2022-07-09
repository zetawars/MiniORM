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
        public static string UpdateQuery<T>(T _Object, string whereClause = null, string tableName = null)
        {
            var properties = GetUpdateProperties<T>();
            StringBuilder UpdateQueryBuilder = new StringBuilder();
            UpdateQueryBuilder.Append($"UPDATE {tableName ?? GetTableName<T>()} SET ");
            foreach (var pi in properties)
            {
                UpdateQueryBuilder.Append($"{GetColumnName(pi)} = {ValueReader(pi.GetValue(_Object))}, ");
            }
            UpdateQueryBuilder.Length -= 2;
            if (string.IsNullOrWhiteSpace(whereClause))
            {
                UpdateQueryBuilder.Append(DefaultWhereClause(_Object));
            }

            UpdateQueryBuilder.Append($" {whereClause}");
            return UpdateQueryBuilder.ToString();
        }
        #endregion Public methods
    }

}

