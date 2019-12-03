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
        private static string DefaultWhereClause<T>(T Object)
        {
            var prop = GetKeyProperty<T>();
            return $" Where {GetColumnName(prop)} = {ValueReader(prop.GetValue(Object))}";
        }
        public static string BeginTransQuery
        {
            get {
                return 
$@"
BEGIN TRY 
BEGIN TRANSACTION ";
            }
        }
        public static string CommitTransQuery
        {
            get
            {
                return
                    $@"
COMMIT 
END TRY 
BEGIN CATCH 
declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
rollback transaction;
raiserror(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH ";
            }
       
        }
        #endregion Public methods

        #region Private methods
     
        private static List<PropertyInfo> GetInsertProperties<T>()
        {
            return typeof(T).GetProperties(
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.DeclaredOnly
                ).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontInsert))) && !(Attribute.IsDefined(x, typeof(PrimaryKey)))).ToList();
        }
        private static List<PropertyInfo> GetUpdateProperties<T>()
        {
            return typeof(T).GetProperties(
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.DeclaredOnly
                ).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontUpdate))) && !(Attribute.IsDefined(x, typeof(PrimaryKey)))).ToList();
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

