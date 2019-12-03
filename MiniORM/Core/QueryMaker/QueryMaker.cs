using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
//Author : Shazaki Zetawars //
namespace MiniORM
{
    public class QueryMaker : DB_Common
    {
        #region Public methods
        public static string SelectQuery<T>(string query = null, string where_clause = null)
        {
            StringBuilder queryBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(query))
            {
                List<PropertyInfo> _properties = GetReadableProperties<T>();
                queryBuilder.Append("SELECT ");
                foreach (var element in _properties)
                {
                    queryBuilder.Append($"[{element.Name}], ");
                }
                queryBuilder.Length -= 2;
                queryBuilder.Append($"FROM {GetTableName<T>()} { where_clause }");
            }
            else
            {
                queryBuilder.Append(query);
                queryBuilder.Append($" { where_clause} ");
            }
            return queryBuilder.ToString();
        }
        public static string InsertQuery<T>(T _Object, string schemaName = null, string tableName = null)
        {
            List<PropertyInfo> properties = GetInsertProperties<T>();
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
        public static string UpdateQuery<T>(T _Object, string whereClause, string schemaName = null, string tableName = null)
        {
            List<PropertyInfo> properties = GetUpdateProperties<T>();
            StringBuilder UpdateQueryBuilder = new StringBuilder();
            UpdateQueryBuilder.Append($"UPDATE {tableName ?? GetTableName<T>()} SET ");
            foreach (var pi in properties)
            {
                UpdateQueryBuilder.Append($"{GetColumnName(pi)} = {ValueReader(pi.GetValue(_Object))}, ");
            }
            UpdateQueryBuilder.Length -= 2;
            UpdateQueryBuilder.Append($" {whereClause}");
            return UpdateQueryBuilder.ToString();
        }
        public static string DeleteQuery<T>(string whereClause, string schemaName = null, string tableName = null)
        {
            return $"Delete from {tableName ?? GetTableName<T>()} {whereClause};";
        }
        public static string BeginTransQuery()
        {
            string query = string.Empty;
            query +=
                "BEGIN TRY " +
                "BEGIN TRANSACTION ";
            return query;
        }
        public static string CommitTransQuery()
        {
            string query = string.Empty;
            query +=
                "COMMIT " +
                "END TRY " +
                "BEGIN CATCH " +
                "declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;" +
                "select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();" +
                "rollback transaction;" +
                "raiserror(@ErrorMessage, @ErrorSeverity, @ErrorState);" +
                "END CATCH ";
            return query;
        }
        public static PagedQuery GetPagerQueries<T>(string query, string sortBy, string order, int offset, int numberOfRecords)
        {
            string mainquery = $"SELECT * FROM ({  query ?? SelectQuery<T>(query, "")}) MyList order by {sortBy} {order} offset {offset} rows fetch next {numberOfRecords} rows only";
            string countquery = $"SELECT COUNT(*) FROM ({  query ?? SelectQuery<T>(query, "") }) MyList";
            return new PagedQuery { Query = mainquery, CountQuery = countquery };
        }
        public static PagedQuery GetPagerQueries<T>(string query, Dictionary<string, string> sortAndOrder, int offset, int numberOfRecords)
        {
            string mainquery = $"SELECT * FROM ({ query ?? SelectQuery<T>(query, "")}) MyList {MultipleOrderByQuery(sortAndOrder, numberOfRecords, offset)}";
            string countquery = $"SELECT COUNT(*) FROM ({ query ?? SelectQuery<T>(query, "")}) MyList";
            return new PagedQuery { Query = mainquery, CountQuery = countquery };
        }
        #endregion Public methods

        #region Private methods

        private static List<PropertyInfo> GetInsertProperties<T>()
        {
            return typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontInsert))) && !(Attribute.IsDefined(x, typeof(Key)))).ToList();
        }
        private static List<PropertyInfo> GetUpdateProperties<T>()
        {
            return typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontUpdate))) && !(Attribute.IsDefined(x, typeof(Key)))).ToList();
        }
        private static string GetColumnName(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(Column)))
            {
                Column k = (Column)Attribute.GetCustomAttribute(pi, typeof(Column));
                return $"[{k.Name}]";
            }
            else
            {
                return $"[{pi.Name}]";
            }
        }
        private static string GetTableName<T>()
        {
            if (Attribute.IsDefined(typeof(T), typeof(Table)))
            {
                Table t = (Table)Attribute.GetCustomAttribute(typeof(T), typeof(Table));
                return $"[{t.SchemaName}].[{t.TableName }]";
            }
            else
            {
                return $"[dbo].[{typeof(T).Name}]";
            }
        }
        private static string ValueReader(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else if (value.GetType() == typeof(DateTime))
            {
                DateTime date = (DateTime)value;
                return $"'{date.ToString("yyyy-MM-dd hh:mm:ss")}'";
            }
            else
            {
                return $"'{value.ToString().Replace("'", "''")}'";
            }
        }
        private static string MultipleOrderByQuery(Dictionary<string, string> sortByAndOrder, int numberOfRecords, int offset)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" order by ");
            foreach (var element in sortByAndOrder)
            {
                sb.Append(element.Key);
                sb.Append($" {element.Value}, ");
            }
            sb.Length -= 2;
            sb.Append($" offset {offset} rows fetch next {numberOfRecords} rows only");
            return sb.ToString();
        }
        #endregion PrivateMethods

    }

}

