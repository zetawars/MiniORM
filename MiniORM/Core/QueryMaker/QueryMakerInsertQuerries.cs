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

        public static string InsertQuery<T>(T _Object, string tableName = null)
        {
            var properties = GetInsertProperties<T>();
            StringBuilder ColumnQueryBuilder = new StringBuilder();
            StringBuilder ValuesQueryBuilder = new StringBuilder();
            ColumnQueryBuilder.Append($"INSERT INTO {tableName ?? GetTableName<T>()}");
            ColumnQueryBuilder.Append("(");
            ValuesQueryBuilder.Append(" VALUES (");
            foreach (var pi in properties)
            {
                ColumnQueryBuilder.Append($"{GetColumnName(pi)} , ");
                ValuesQueryBuilder.Append($"{ValueReader(pi.GetValue(_Object))}, ");
            }
            ColumnQueryBuilder.Length -= 2;
            ColumnQueryBuilder.Append(")");
            ValuesQueryBuilder.Length -= 2;
            ValuesQueryBuilder.Append(")");
            ColumnQueryBuilder.Append(ValuesQueryBuilder.ToString());
            return ColumnQueryBuilder.ToString();
        }

        #endregion Public methods



    }

}

