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
        public static string SelectQuery<T>(string query = null, string whereClause = null)
        {
            var queryBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(query))
            {
                List<PropertyInfo> _properties = GetReadableProperties<T>();
                queryBuilder.Append("SELECT ");
                foreach (var element in _properties)
                {
                    queryBuilder.Append($"[{element.Name}], ");
                }
                queryBuilder.Length -= 2;
                queryBuilder.Append($" FROM {GetTableName<T>()}");
            }
            else
            {
                queryBuilder.Append(query);
            }
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                queryBuilder.Append($" {whereClause}");
            }

            return queryBuilder.ToString();
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


    }
}

